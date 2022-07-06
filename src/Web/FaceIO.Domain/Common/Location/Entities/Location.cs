namespace FaceIO.Domain.Common.Location.Entities
{
    public class Location
    {
        public int Id { get; protected internal set; }

        public Guid Uid { get; protected internal set; }

        public DateTime CreatedOn { get; protected internal set; }

        public DateTime? DeletedOn { get; protected internal set; }

        public string Name { get; protected internal set; } = null!;
    }
}