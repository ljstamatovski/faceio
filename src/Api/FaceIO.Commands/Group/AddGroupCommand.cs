namespace FaceIO.Commands.Group
{
    using Contracts.Common.Database.Context;
    using Domain.Customer.Repositories;
    using Domain.Customer.Entities;
    using Domain.Group.Entities;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddGroupCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public string Name { get; }

        public string? Description { get; }

        public AddGroupCommand(Guid customerUid, string name, string? description)
        {
            CustomerUid = customerUid;
            Name = name;
            Description = description;
        }
    }

    public class AddGroupCommandHandler : IRequestHandler<AddGroupCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly ICustomersRepository _customersRepository;

        public AddGroupCommandHandler(IFaceIODbContext dbContext, ICustomersRepository customersRepository)
        {
            _dbContext = dbContext;
            _customersRepository = customersRepository;
        }

        public async Task<Unit> Handle(AddGroupCommand request, CancellationToken cancellationToken)
        {
            Customer customer = await _customersRepository.GetCustomerAsync(request.CustomerUid);

            Group group = Group.Factory.Create(name: request.Name,
                                               description: request.Description,
                                               customerId: customer.Id);

            _dbContext.Set<Group>().Add(group);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
