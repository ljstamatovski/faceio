namespace FaceIO.Domain.Location.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Location : Entity
    {
        public string Name { get; protected internal set; } = string.Empty;

        public string? Description { get; protected internal set; }

        public int CustomerFk { get; protected internal set; }

        [ForeignKey(nameof(CustomerFk))]
        public Customer Customer { get; protected internal set; } = null!;

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

        public static class Factory
        {
            public static Location Create(string name, string? description, int customerId)
            {
                return new Location
                {
                    Uid = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    Name = name,
                    Description = description,
                    CustomerFk = customerId
                };
            }
        }
    }
}