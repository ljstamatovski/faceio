namespace FaceIO.Domain.Common.Entities
{
    using System;

    public class Entity
    {
        public int Id { get; protected internal set; }

        public Guid Uid { get; protected internal set; }

        public DateTime CreatedOn { get; protected internal set; }

        public DateTime? DeletedOn { get; protected internal set; }
    }
}