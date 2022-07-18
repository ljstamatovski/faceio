namespace FaceIO.Commands.Location
{
    using Contracts.Common.Database.Context;
    using Domain.Customer.Entities;
    using Domain.Customer.Repositories;
    using Domain.Location.Entities;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddLocationCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public string Name { get; }

        public string? Description { get; }

        public AddLocationCommand(Guid customerUid, string name, string? description)
        {
            Name = name;
            Description = description;
            CustomerUid = customerUid;
        }
    }

    public class AddLocationCommandHandler : IRequestHandler<AddLocationCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly ICustomersRepository _customersRepository;

        public AddLocationCommandHandler(IFaceIODbContext dbContext, ICustomersRepository customersRepository)
        {
            _dbContext = dbContext;
            _customersRepository = customersRepository;
        }

        public async Task<Unit> Handle(AddLocationCommand request, CancellationToken cancellationToken)
        {
            Customer customer = await _customersRepository.GetCustomerAsync(request.CustomerUid);

            Location location = Location.Factory.Create(name: request.Name,
                                                        request.Description,
                                                        customer.Id);

            customer.Locations.Add(location);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}