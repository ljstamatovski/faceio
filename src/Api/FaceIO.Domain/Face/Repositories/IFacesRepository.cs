namespace FaceIO.Domain.Face.Repositories
{
    using Entities;

    public interface IFacesRepository
    {
        /// <summary>
        /// Returns face for provided customer, person uid and face uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="personUid"> Uid of the person. </param>
        /// <param name="faceUid"> Uid of the face. </param>
        Task<Face> GetFaceAsync(Guid customerUid, Guid personUid, Guid faceUid);
    }
}