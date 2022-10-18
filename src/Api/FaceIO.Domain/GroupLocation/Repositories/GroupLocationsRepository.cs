namespace FaceIO.Domain.GroupLocation.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Exceptions;
    using Customer.Entities;
    using Entities;
    using Group.Entities;
    using Location.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class GroupLocationsRepository : Repository<GroupLocation>, IGroupLocationsRepository
    {
        public GroupLocationsRepository(IFaceIODbContext dbContext) : base(dbContext)
        {
        }

        public async Task<GroupLocation> GetGroupLocationAsync(Guid customerUid, Guid groupUid, Guid locationUid, Guid groupLocationUid)
        {
            GroupLocation? groupLocation = await (from dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                                                  join dbLocation in All<Location>().Where(x => x.Uid == locationUid)
                                                  on dbCustomer.Id equals dbLocation.CustomerFk
                                                  join dbGroupLocation in All<GroupLocation>().Where(x => x.Uid == groupLocationUid)
                                                  on dbLocation.Id equals dbGroupLocation.LocationFk
                                                  join dbGroup in All<Group>().Where(x => x.Uid == groupUid)
                                                  on dbGroupLocation.GroupFk equals dbGroup.Id
                                                  select dbGroupLocation).SingleOrDefaultAsync();

            if (groupLocation is null)
            {
                throw new FaceIONotFoundException($"Group location with uid {groupLocationUid} not found.");
            }

            return groupLocation;
        }
    }
}