namespace FaceIO.Domain.GroupLocation.Entities
{
    using Common.Entities;
    using Group.Entities;
    using Location.Entities;
    using System.ComponentModel.DataAnnotations.Schema;

    public class GroupLocation : Entity
    {
        public int GroupFk { get; set; }

        [ForeignKey(nameof(GroupFk))]
        public Group Group { get; set; } = null!;

        public int LocationFk { get; set; }

        [ForeignKey(nameof(LocationFk))]
        public Location Location { get; set; } = null!;
    }
}