namespace FaceIO.Domain.Person.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public sealed class Person : Entity
    {
        public string Name { get; internal set; } = string.Empty;

        public string? FileName { get; internal set; }

        public string? Email { get; internal set; }

        public string? Phone { get; internal set; }

        public int CustomerFk { get; internal set; }

        [ForeignKey(nameof(CustomerFk))]
        public Customer Customer { get; set; } = null!;

        public Person SetName(string name)
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not set name, person is deleted.");
            }

            Name = name;

            return this;
        }

        public Person SetEmail(string? email)
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not set email, person is deleted.");
            }

            Email = email;

            return this;
        }

        public Person SetPhone(string? phone)
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not set phone, person is deleted.");
            }

            Phone = phone;

            return this;
        }

        public Person SetFileName(Guid customerUid, string fileName)
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not set bucket key, person is deleted.");
            }

            FileName = $"{customerUid}/{Uid}/{fileName}";

            return this;
        }

        public void MarkAsDeleted()
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not delete person, person is already deleted.");
            }

            DeletedOn = DateTime.UtcNow;
        }

        public static class Factory
        {
            public static Person Create(
                string name,
                string? email,
                string? phone,
                int customerId)
            {
                return new Person
                {
                    Uid = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    Name = name,
                    Email = email,
                    Phone = phone,
                    CustomerFk = customerId
                };
            }
        }
    }
}