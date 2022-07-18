﻿namespace FaceIO.Domain.Group.Repositories
{
    using Group.Entities;

    public interface IGroupsRepository
    {
        /// <summary>
        /// Returns group for provided customer and group uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        /// <param name="groupUid"> Uid of the group. </param>
        Task<Group> GetGroupAsync(Guid customerUid, Guid groupUid);
    }
}