namespace FaceIO.Commands.Group
{
    using Contracts.Common.Database.Context;
    using Domain.Group.Entities;
    using Domain.Group.Repositories;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateGroupCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public string Name { get; }

        public string? Description { get; }

        public UpdateGroupCommand(Guid customerUid, Guid groupUid, string name, string? description)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            Name = name;
            Description = description;
        }
    }

    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IGroupsRepository _groupsRepository;

        public UpdateGroupCommandHandler(IFaceIODbContext dbContext, IGroupsRepository groupsRepository)
        {
            _dbContext = dbContext;
            _groupsRepository = groupsRepository;
        }

        public async Task<Unit> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            Group group = await _groupsRepository.GetGroupAsync(request.CustomerUid, request.GroupUid);

            group.SetName(request.Name)
                 .SetDescription(request.Description);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
