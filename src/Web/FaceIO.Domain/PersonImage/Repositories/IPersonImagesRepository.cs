namespace FaceIO.Domain.PersonImage.Repositories
{
    using Entities;

    public interface IPersonImagesRepository
    {
        /// <summary>
        /// Returns person image for provided customer, person uid and person image uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="personUid"> Uid of the person. </param>
        /// <param name="personImageUid"> Uid of the person image. </param>
        Task<PersonImage> GetPersonImageAsync(Guid customerUid, Guid personUid, Guid personImageUid);
    }
}