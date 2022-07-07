namespace FaceIO.Domain.Person.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Person : Entity
    {
        public string Name { get; protected internal set; } = string.Empty;

        public int CustomerFk { get; protected internal set; }

        [ForeignKey(nameof(CustomerFk))]
        public Customer Customer { get; protected internal set; } = null!;
    }
}