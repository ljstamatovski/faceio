namespace FaceIO.Contracts.GroupLocation
{
    using Common;
    using System;

    public class GroupLocationDto : RestBase
    {
        public Guid GroupUid { get; set; }

        public Guid LocationUid { get; set; }
    }
}