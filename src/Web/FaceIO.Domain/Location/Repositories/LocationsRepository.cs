namespace FaceIO.Domain.Location.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Exceptions;
    using Customer.Entities;
    using Entities;
    using Group.Entities;
    using GroupLocation.Entities;
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

        public async Task<object> GetGroupsWithLocationAccessAsync(Guid customerUid, Guid locationUid)
        {
            var p = await (from dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                           join dbLocation in All<Location>().Where(x => x.Uid == locationUid)
                           on dbCustomer.Id equals dbLocation.CustomerFk
                           join dbGroupLocation in All<GroupLocation>()
                           on dbLocation.Id equals dbGroupLocation.LocationFk
                           join dbGroup in All<Group>().Include(x => x.PersonsInGroup)
                                                       .ThenInclude(x => x.Person)
                                                       .ThenInclude(x => x.PersonImage)
                           on dbGroupLocation.GroupFk equals dbGroup.Id
                           select new
                           {
                               dbGroup.Name,
                               dbGroup.Description,
                               Persons = dbGroup.PersonsInGroup.Select(dbPersonInGroup => new
                               {
                                   dbPersonInGroup.Person.Name,
                                   dbPersonInGroup.Person.PersonImage.FileName
                               }).ToList()
                           }).ToListAsync();

            return p;
        }
    }
}