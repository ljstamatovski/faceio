namespace FaceIO.Commands.Customer
{
    using Contracts.Common.Database.Context;
    using Domain.Customer.Entities;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddCustomerCommand : IRequest
    {
        public string Name { get; }

        public AddCustomerCommand(string name)
        {
            Name = name;
        }
    }

    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand>
    {
        private readonly IFaceIODbContext _dbContext;

        public AddCustomerCommandHandler(IFaceIODbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer customer = Customer.Factory.Create(name: request.Name);

            _dbContext.Set<Customer>().Add(customer);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}