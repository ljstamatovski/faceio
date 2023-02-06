namespace FaceIO.Domain.Group.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Exceptions;
    using Customer.Entities;
    using Group.Entities;
    using GroupAccessToLocation.Entities;
    using Microsoft.EntityFrameworkCore;

    public class GroupsRepository : Repository<Group>, IGroupsRepository
    {
        public GroupsRepository(IFaceIODbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Group> GetGroupAsync(Guid customerUid, Guid groupUid)
        {
            Group? group = await (from dbGroup in All<Group>().Where(x => x.Uid == groupUid)
                                  join dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                                  on dbGroup.CustomerFk equals dbCustomer.Id
                                  select dbGroup).SingleOrDefaultAsync();

            if (group is null)
            {
                throw new FaceIONotFoundException($"Group with uid {groupUid} not found.");
            }

            return group;
        }

        public async Task<bool> HasGroupAccessToLocationAsync(Guid customerUid, Guid groupUid)
        {
            return await (from dbGroup in All<Group>().Where(x => x.Uid == groupUid)
                          join dbCustomer in All<Customer>().Where(x => x.Uid == customerUid)
                          on dbGroup.CustomerFk equals dbCustomer.Id
                          join dbGroupAccessToLocation in All<GroupAccessToLocation>()
                          on dbGroup.Id equals dbGroupAccessToLocation.GroupFk
                          select dbGroupAccessToLocation.Id).AnyAsync();
        }
    }
}