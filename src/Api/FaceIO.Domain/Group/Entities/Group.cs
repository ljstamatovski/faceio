﻿namespace FaceIO.Domain.Group.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using PersonInGroup.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public sealed class Group : Entity
    {
        public string Name { get; internal set; } = string.Empty;

        public string? Description { get; internal set; }

        public int CustomerFk { get; internal set; }

        [ForeignKey(nameof(CustomerFk))]
        public Customer Customer { get; set; } = null!;

        public ICollection<PersonInGroup> PersonsInGroup { get; set; } = new List<PersonInGroup>();

        public Group SetName(string name)
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not set name, group is deleted.");
            }

            Name = name;

            return this;
        }

        public Group SetDescription(string? description)
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not set description, group is deleted.");
            }

            Description = description;

            return this;
        }

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