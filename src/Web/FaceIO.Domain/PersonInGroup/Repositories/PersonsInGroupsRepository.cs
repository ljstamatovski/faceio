namespace FaceIO.Domain.PersonInGroup.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Exceptions;
    using Customer.Entities;
    using Group.Entities;
    using Microsoft.EntityFrameworkCore;
    using Person.Entities;
    using PersonInGroup.Entities;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class PersonsInGroupsRepository : Repository<PersonInGroup>, IPersonsInGroupsRepository
    {
        public PersonsInGroupsRepository(IFaceIODbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PersonInGroup> GetPersonInGroupAsync(Guid customerUid, Guid groupUid, Guid personUid, Guid personInGroupUid)
        {
            PersonInGroup? personInGroup = await (from dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                                                  join dbGroup in All<Group>().Where(x => x.Uid == groupUid)
                                                  on dbCustomer.Id equals dbGroup.CustomerFk
                                                  join dbPersonInGroup in All<PersonInGroup>().Where(x => x.Uid == personInGroupUid)
                                                  on dbGroup.Id equals dbPersonInGroup.GroupFk
                                                  join dbPerson in All<Person>().Where(x => x.Uid == personUid)
                                                  on dbPersonInGroup.PersonFk equals dbPerson.Id
                                                  select dbPersonInGroup).SingleOrDefaultAsync();

            if (personInGroup is null)
            {
                throw new FaceIONotFoundException($"Person in group with uid {personInGroupUid} not found.");
            }

            return personInGroup;
        }
    }
}