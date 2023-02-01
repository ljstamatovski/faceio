namespace FaceIO.Domain.Location.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using GroupLocation.Entities;
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

        public ICollection<GroupLocation> LocationGroups { get; protected internal set; }

        public Location()
        {
            LocationGroups = new List<GroupLocation>();
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

        public void AddGroup(int groupId)
        {
            var groupLocation = GroupLocation.Factory.Create(groupFk: groupId, locationFk: Id);

            LocationGroups.Add(groupLocation);
        }

        public void RemoveGroup(Guid groupUid)
        {
            GroupLocation locationGroup = LocationGroups.Single(x => x.Group.Uid == groupUid);

            locationGroup.MarkAsDeleted();
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

                location.CollectionId = $"{customer.Uid}/{location.Uid}";

                return location;
            }
        }
    }
}