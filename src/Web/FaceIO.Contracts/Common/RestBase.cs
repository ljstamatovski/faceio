namespace FaceIO.Contracts.Common
{
    using System;

    public class RestBase
    {
        public int Id { get; set; }

        public Guid Uid { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}