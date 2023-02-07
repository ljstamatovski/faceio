namespace FaceIO.Domain.Location.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using FaceIO.Contracts.Common.Exceptions;
    using GroupAccessToLocation.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Location : Entity
    {
        public string Name { get; protected internal set; } = string.Empty;

        public string? Description { get; protected internal set; }

        public string CollectionId { get; protected internal set; } = string.Empty;

        public int CustomerFk { get; protected internal set; }

        [ForeignKey(nameof(CustomerFk))]
        public Customer Customer { get; protected internal set; } = null!;

        public ICollection<GroupAccessToLocation> GroupsWithAccessToLocation { get; protected internal set; }

        public Location()
        {
            GroupsWithAccessToLocation = new List<GroupAccessToLocation>();
        }

        public Location SetName(string name)
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not set name, location is deleted.");
            }

            Name = name;

            return this;
        }

        public Location SetDescription(string? description)
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not set description, location is deleted.");
            }

            Description = description;

            return this;
        }

        public void MarkAsDeleted()
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not delete location, location is already deleted.");
            }

            DeletedOn = DateTime.UtcNow;
        }

        public GroupAccessToLocation AddGroup(int groupId)
        {
            var groupAccessToLocation = GroupAccessToLocation.Factory.Create(groupFk: groupId, locationFk: Id);

            GroupsWithAccessToLocation.Add(groupAccessToLocation);

            return groupAccessToLocation;
        }

        public void RemoveGroup(Guid groupUid)
        {
            GroupAccessToLocation groupAccessToLocation = GetGroupAccessToLocation(groupUid);

            groupAccessToLocation.MarkAsDeleted();
        }

        public GroupAccessToLocation GetGroupAccessToLocation(Guid groupUid)
        {
            GroupAccessToLocation? groupAccessToLocation = GroupsWithAccessToLocation.SingleOrDefault(x => x.Group.Uid == groupUid);

            if (groupAccessToLocation is null)
            {
                throw new FaceIONotFoundException($"Group with uid {groupUid} not found.");
            }

            return groupAccessToLocation;
        }

        public static class Factory
        {
            public static Location Create(string name, string? description, Customer customer)
            {
                var location = new Location
                {
                    Uid = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    Name = name,
                    Description = description,
                    Customer = customer
                };

                location.CollectionId = $"collection_{location.Uid}";

                return location;
            }
        }
    }
}