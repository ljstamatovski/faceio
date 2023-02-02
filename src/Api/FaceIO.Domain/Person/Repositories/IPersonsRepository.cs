namespace FaceIO.Domain.Person.Repositories
{
    using Entities;
    using PersonAccessToLocation.Entities;

    public interface IPersonsRepository
    {
        /// <summary>
        /// Returns person for provided customer and person uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="personUid"> Uid of the person. </param>
        Task<Person> GetPersonAsync(Guid customerUid, Guid personUid);

        /// <summary>
        /// Returns persons in provided the group uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="groupUid"> Uid of the group. </param>
        Task<IReadOnlyList<Person>> GetPersonsInGroupAsync(Guid customerUid, Guid groupUid);

        /// <summary>
        /// Returns persons with access to provided location through the given group.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="locationUid"> Uid of the location. </param>
        /// <param name="groupUid"> Uid of the group. </param>
        /// <returns></returns>
        Task<IReadOnlyList<PersonAccessToLocation>> GetPersonsAccessToLocationAsync(Guid customerUid, Guid locationUid, Guid groupUid);
    }
}