namespace FaceIO.Domain.Location.Repositories
{
    using Entities;
    using FaceIO.Domain.PersonAccessToLocation.Entities;

    public interface ILocationsRepository
    {
        /// <summary>
        /// Returns location for provided customer and location uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="locationUid"> Uid of the location. </param>
        Task<Location> GetLocationAsync(Guid customerUid, Guid locationUid);

        /// <summary>
        /// Returns locations for the provided customer and group uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="groupUid"> Uid of the group. </param>
        Task<IReadOnlyList<Location>> GetGroupLocationsAsync(Guid customerUid, Guid groupUid);
    }
}