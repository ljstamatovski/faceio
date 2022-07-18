namespace FaceIO.Domain.Location.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Customer.Entities;
    using Entities;
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
                throw new Exception("Location not found.");
            }

            return location;
        }
    }
}