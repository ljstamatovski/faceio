namespace FaceIO.Commands.PersonInGroup
{
    using Contracts.Common.Database.Context;
    using Domain.PersonInGroup.Entities;
    using Domain.PersonInGroup.Repositories;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemovePersonFromGroupCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public Guid PersonUid { get; }

        public Guid PersonInGroupUid { get; }

        public RemovePersonFromGroupCommand(Guid customerUid, Guid groupUid, Guid personUid, Guid personInGroupUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            PersonUid = personUid;
            PersonInGroupUid = personInGroupUid;
        }
    }

    public class RemovePersonFromGroupCommandHandler : IRequestHandler<RemovePersonFromGroupCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IPersonsInGroupsRepository _personsInGroupsRepository;

        public RemovePersonFromGroupCommandHandler(IFaceIODbContext dbContext, IPersonsInGroupsRepository personsInGroupsRepository)
        {
            _dbContext = dbContext;
            _personsInGroupsRepository = personsInGroupsRepository;
        }

        public async Task<Unit> Handle(RemovePersonFromGroupCommand command, CancellationToken cancellationToken)
        {
            PersonInGroup personInGroup = await _personsInGroupsRepository.GetPersonInGroupAsync(
                customerUid: command.CustomerUid,
                groupUid: command.GroupUid,
                personUid: command.PersonUid,
                personInGroupUid: command.PersonInGroupUid);

            personInGroup.MarkAsDeleted();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}