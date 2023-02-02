namespace FaceIO.Domain.Location.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Exceptions;
    using Customer.Entities;
    using Entities;
    using FaceIO.Domain.Group.Entities;
    using FaceIO.Domain.GroupAccessToLocation.Entities;
    using FaceIO.Domain.Person.Entities;
    using FaceIO.Domain.PersonInGroup.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class LocationsRepository : Repository<Location>, ILocationsRepository
    {
        public LocationsRepository(IFaceIODbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<Location> GetLocationAsync(Guid customerUid, Guid locationUid)
        {
            Location? location = await (from dbLocation in All<Location>().Where(x => x.Uid == locationUid)
                                        join dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                                        on dbLocation.CustomerFk equals dbCustomer.Id
                                        select dbLocation).SingleOrDefaultAsync();

            if (location is null)
            {
                throw new FaceIONotFoundException($"Location with uid {locationUid} not found.");
            }

            return location;
        }

        public async Task<IReadOnlyList<Location>> GetPersonLocationsAsync(Guid customerUid, Guid personUid)
        {
            return await (from dbPerson in All<Person>().Where(x => x.Uid == personUid)
                          join dbPersonInGroup in All<PersonInGroup>()
                          on dbPerson.Id equals dbPersonInGroup.PersonFk
                          join dbGroup in All<Group>()
                          on dbPersonInGroup.GroupFk equals dbGroup.Id
                          join dbGroupAccessToLocation in All<GroupAccessToLocation>()
                          on dbGroup.Id equals dbGroupAccessToLocation.GroupFk
                          join dbLocation in All<Location>()
                          on dbGroupAccessToLocation.LocationFk equals dbLocation.Id
                          select dbLocation).ToArrayAsync();
        }
    }
}