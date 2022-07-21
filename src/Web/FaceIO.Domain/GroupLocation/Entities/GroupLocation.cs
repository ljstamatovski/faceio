﻿namespace FaceIO.Domain.GroupLocation.Entities
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

        public static class Factory
        {
            public static GroupLocation Create(int groupFk, int locationFk)
            {
                return new GroupLocation
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