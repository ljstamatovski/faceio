namespace FaceIO.Commands.Person
{
    using Contracts.Common.Database.Context;
    using Domain.Customer.Entities;
    using Domain.Customer.Repositories;
    using Domain.Person.Entities;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddPersonCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public string Name { get; }

        public AddPersonCommand(Guid customerUid, string name)
        {
            Name = name;
            CustomerUid = customerUid;
        }
    }

    public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly ICustomersRepository _customersRepository;

        public AddPersonCommandHandler(IFaceIODbContext dbContext, ICustomersRepository customersRepository)
        {
            _dbContext = dbContext;
            _customersRepository = customersRepository;
        }

        public async Task<Unit> Handle(AddPersonCommand request, CancellationToken cancellationToken)
        {
            Customer customer = await _customersRepository.GetCustomerAsync(request.CustomerUid);

            Person person = Person.Factory.Create(name: request.Name, customerId: customer.Id);

            customer.Persons.Add(person);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
