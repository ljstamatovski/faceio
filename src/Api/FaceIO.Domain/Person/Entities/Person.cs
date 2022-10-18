namespace FaceIO.Domain.Person.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using PersonImage.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public sealed class Person : Entity
    {
        public string Name { get; internal set; } = string.Empty;

        public int CustomerFk { get; internal set; }

        [ForeignKey(nameof(CustomerFk))]
        public Customer Customer { get; } = null!;

        public int PersonImageFk { get; set; }

        [ForeignKey(nameof(PersonImageFk))]
        public PersonImage PersonImage { get; set; } = null!;

        public Person SetName(string name)
        {
            if (DeletedOn.HasValue)
            {
                throw new ValidationException("Can not set name, person is deleted.");
            }

            Name = name;

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
            public static Person Create(string name, int customerId)
            {
                return new Person
                {
                    Uid = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    Name = name,
                    CustomerFk = customerId
                };
            }
        }
    }
}