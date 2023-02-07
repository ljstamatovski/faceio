namespace FaceIO.Domain.Person.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Customer.Entities;
    using Entities;
    using FaceIO.Contracts.Common.Exceptions;
    using FaceIO.Domain.GroupAccessToLocation.Entities;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.PersonAccessToLocation.Entities;
    using Group.Entities;
    using Microsoft.EntityFrameworkCore;
    using PersonInGroup.Entities;

    public class PersonsRepository : Repository<Person>, IPersonsRepository
    {
        public PersonsRepository(IFaceIODbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Person> GetPersonAsync(Guid customerUid, Guid personUid)
        {
            Person? person = await (from dbPerson in All<Person>().Where(x => x.Uid == personUid)
                                    join dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                                    on dbPerson.CustomerFk equals dbCustomer.Id
                                    select dbPerson).SingleOrDefaultAsync();

            if (person is null)
            {
                throw new FaceIONotFoundException($"Person with uid {personUid} not found.");
            }

            return person;
        }

        public async Task<IReadOnlyList<Person>> GetPersonsInGroupAsync(Guid customerUid, Guid groupUid)
        {
            return await (from dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                          join dbGroup in All<Group>().Where(x => x.Uid == groupUid)
                          on dbCustomer.Id equals dbGroup.CustomerFk
                          join dbPersonInGroup in All<PersonInGroup>()
                          on dbGroup.Id equals dbPersonInGroup.GroupFk
                          join dbPerson in All<Person>()
                          on dbPersonInGroup.PersonFk equals dbPerson.Id
                          select dbPerson).ToArrayAsync();
        }

        public async Task<IReadOnlyList<PersonAccessToLocation>> GetPersonsAccessToLocationAsync(Guid customerUid, Guid locationUid, Guid groupUid)
        {
            return await (from dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                          join dbGroup in All<Group>().Where(x => x.Uid == groupUid)
                          on dbCustomer.Id equals dbGroup.CustomerFk
                          join dbGroupAccessToLocation in All<GroupAccessToLocation>()
                          on dbGroup.Id equals dbGroupAccessToLocation.GroupFk
                          join dbLocation in All<Location>().Where(x => x.Uid == locationUid)
                          on dbGroupAccessToLocation.LocationFk equals dbLocation.Id
                          join dbPersonAccessToLocation in All<PersonAccessToLocation>()
                          on dbGroupAccessToLocation.Id equals dbPersonAccessToLocation.GroupAccessToLocationFk
                          select dbPersonAccessToLocation).ToArrayAsync();
        }

        public async Task<IReadOnlyList<PersonAccessToLocation>> GetPersonAccessToLocationsAsync(Guid customerUid, Guid groupUid, Guid personUid)
        {
            return await (from dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                          join dbPerson in All<Person>().Where(x => x.Uid == personUid)
                          on dbCustomer.Id equals dbPerson.CustomerFk
                          join dbPersonInGroup in All<PersonInGroup>()
                          on dbPerson.Id equals dbPersonInGroup.PersonFk
                          join dbGroup in All<Group>().Where(x => x.Uid == groupUid)
                          on dbPersonInGroup.GroupFk equals dbGroup.Id
                          join dbGroupAccessToLocation in All<GroupAccessToLocation>()
                          on dbGroup.Id equals dbGroupAccessToLocation.GroupFk
                          join dbPersonAccessToLocation in All<PersonAccessToLocation>()
                          on dbGroupAccessToLocation.Id equals dbPersonAccessToLocation.GroupAccessToLocationFk
                          select dbPersonAccessToLocation).Include(x => x.GroupAccessToLocation)
                                                          .ThenInclude(x => x.Location)
                                                          .ToArrayAsync();
        }
    }
}