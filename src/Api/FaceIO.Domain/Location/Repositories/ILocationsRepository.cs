namespace FaceIO.Domain.Location.Repositories
{
    using Entities;

    public interface ILocationsRepository
    {
        /// <summary>
        /// Returns location for provided customer and location uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="locationUid"> Uid of the location. </param>
        Task<Location> GetLocationAsync(Guid customerUid, Guid locationUid);

        /// <summary>
        /// Returns locations for the provided customer and location uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="personUid"> Uid of the person. </param>
        Task<IReadOnlyList<Location>> GetPersonLocationsAsync(Guid customerUid, Guid personUid);
    }
}