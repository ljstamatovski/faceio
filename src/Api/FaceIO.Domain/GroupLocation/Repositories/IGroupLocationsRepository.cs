namespace FaceIO.Domain.GroupLocation.Repositories
{
    using Entities;

    public interface IGroupLocationsRepository
    {
        /// <summary>
        /// Returns group location for provided customer, group, location and group location uids.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="groupUid"> Uid of the group. </param>
        /// <param name="locationUid"> Uid of the location. </param>
        /// <param name="groupLocationUid"> Uid of the group location. </param>
        Task<GroupLocation> GetGroupLocationAsync(Guid customerUid, Guid groupUid, Guid locationUid, Guid groupLocationUid);
    }
}