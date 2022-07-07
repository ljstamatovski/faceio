namespace FaceIO.Commands.Customer
{
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Domain.Customer.Entities;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddCustomerCommand : IRequest<Customer>
    {
        public string Name { get; set; }

        public AddCustomerCommand(string name)
        {
            Name = name;
        }
    }

    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, Customer>
    {
        private readonly IFaceIODbContext _dbContext;

        public AddCustomerCommandHandler(IFaceIODbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Uid = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                Name = request.Name
            };

            _dbContext.Set<Customer>().Add(customer);

            await _dbContext.SaveChangesAsync();

            return customer;
        }
    }
}