namespace FaceIO.Commands.PersonInGroup
{
    using Contracts.Common.Database.Context;
    using Domain.Group.Entities;
    using Domain.Group.Repositories;
    using Domain.Person.Entities;
    using Domain.Person.Repositories;
    using FaceIO.Domain.PersonInGroup.Entities;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddPersonInGroupCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public Guid PersonUid { get; }

        public AddPersonInGroupCommand(Guid customerUid, Guid groupUid, Guid personUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            PersonUid = personUid;
        }
    }

    public class AddPersonInGroupCommandHandler : IRequestHandler<AddPersonInGroupCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IGroupsRepository _groupsRepository;
        private readonly IPersonsRepository _personsRepository;

        public AddPersonInGroupCommandHandler(IFaceIODbContext dbContext, IGroupsRepository groupsRepository, IPersonsRepository personsRepository)
        {
            _dbContext = dbContext;
            _groupsRepository = groupsRepository;
            _personsRepository = personsRepository;
        }

        public async Task<Unit> Handle(AddPersonInGroupCommand command, CancellationToken cancellationToken)
        {
            Group group = await _groupsRepository.GetGroupAsync(customerUid: command.CustomerUid, groupUid: command.GroupUid);

            Person person = await _personsRepository.GetPersonAsync(customerUid: command.CustomerUid, personUid: command.PersonUid);

            PersonInGroup personInGroup = PersonInGroup.Factory.Create(groupId: group.Id, personId: person.Id);

            _dbContext.Set<PersonInGroup>().Add(personInGroup);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}