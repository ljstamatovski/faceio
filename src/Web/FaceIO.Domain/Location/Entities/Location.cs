namespace FaceIO.Domain.Location.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Location : Entity
    {
        public string Name { get; set; } = string.Empty;

        public int CustomerFk { get; set; }

        [ForeignKey(nameof(CustomerFk))]
        public Customer Customer { get; set; } = null!;
    }
}