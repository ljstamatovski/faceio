namespace FaceIO.Domain.PersonInGroup.Repositories
{
    using Entities;
    using System;
    using System.Threading.Tasks;

    public interface IPersonsInGroupsRepository
    {
        /// <summary>
        /// Returns person in group for the provided customer, group, person and person in group uids.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="groupUid"> Uid of the group. </param>
        /// <param name="personUid"> Uid of the person. </param>
        /// <param name="personInGroupUid"> Uid of the person in group. </param>
        Task<PersonInGroup> GetPersonInGroupAsync(Guid customerUid, Guid groupUid, Guid personUid, Guid personInGroupUid);
    }
}