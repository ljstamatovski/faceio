namespace FaceIO.Domain.GroupAccessToLocation.Entities
{
    using Common.Entities;
    using Group.Entities;
    using Location.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class GroupAccessToLocation : Entity
    {
        public int GroupFk { get; protected internal set; }

        [ForeignKey(nameof(GroupFk))]
        public Group Group { get; protected internal set; } = null!;

        public int LocationFk { get; protected internal set; }

        [ForeignKey(nameof(LocationFk))]
        public Location Location { get; protected internal set; } = null!;

        public void MarkAsDeleted()
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not delete group access to location, group access to location is already deleted.");
            }

            DeletedOn = DateTime.UtcNow;
        }

        public static class Factory
        {
            public static GroupAccessToLocation Create(int groupFk, int locationFk)
            {
                return new GroupAccessToLocation
                {
                    Uid = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    GroupFk = groupFk,
                    LocationFk = locationFk
                };
            }
        }
    }
}