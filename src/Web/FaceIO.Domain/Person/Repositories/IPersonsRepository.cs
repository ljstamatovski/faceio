namespace FaceIO.Domain.Person.Repositories
{
    using Entities;

    public interface IPersonsRepository
    {
        /// <summary>
        /// Returns group for provided customer and person uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="personUid"> Uid of the person. </param>
        Task<Person> GetPersonAsync(Guid customerUid, Guid personUid);
    }
}