namespace FaceIO.Domain.Group.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Group : Entity
    {
        public string Name { get; protected internal set; } = string.Empty;

        public string? Description { get; protected internal set; }

        public int CustomerFk { get; protected internal set; }

        [ForeignKey(nameof(CustomerFk))]
        public Customer Customer { get; protected internal set; } = null!;

        public void MarkAsDeleted()
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not delete group, group is already deleted.");
            }

            DeletedOn = DateTime.UtcNow;
        }

        public static class Factory
        {
            public static Group Create(string name, string? description, int customerId)
            {
                return new Group
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