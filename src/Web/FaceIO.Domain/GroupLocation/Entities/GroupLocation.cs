namespace FaceIO.Domain.GroupLocation.Entities
{
    using Common.Entities;
    using Group.Entities;
    using Location.Entities;
    using System.ComponentModel.DataAnnotations.Schema;

    public class GroupLocation : Entity
    {
        public int GroupFk { get; protected internal set; }

        [ForeignKey(nameof(GroupFk))]
        public Group Group { get; protected internal set; } = null!;

        public int LocationFk { get; protected internal set; }

        [ForeignKey(nameof(LocationFk))]
        public Location Location { get; protected internal set; } = null!;
    }
}