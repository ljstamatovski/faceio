namespace FaceIO.Commands.Group
{
    using Contracts.Common.Database.Context;
    using Domain.Group.Repositories;
    using Domain.Group.Entities;
    using MediatR;
    using System;

    public class RemoveGroupCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public RemoveGroupCommand(Guid customerUid, Guid groupUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
        }
    }

    public class RemoveGroupCommandHandler : IRequestHandler<RemoveGroupCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IGroupsRepository _groupsRepository;

        public RemoveGroupCommandHandler(IFaceIODbContext dbContext, IGroupsRepository groupsRepository)
        {
            _dbContext = dbContext;
            _groupsRepository = groupsRepository;
        }

        public async Task<Unit> Handle(RemoveGroupCommand request, CancellationToken cancellationToken)
        {
            Group group = await _groupsRepository.GetGroupAsync(request.CustomerUid, request.GroupUid);

            group.MarkAsDeleted();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}