namespace FaceIO.Domain.Group.Entities
{
    using Common.Entities;
    using Customer.Entities;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Group : Entity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int CustomerFk { get; set; }

        [ForeignKey(nameof(CustomerFk))]
        public Customer Customer { get; set; } = null!;
    }
}
