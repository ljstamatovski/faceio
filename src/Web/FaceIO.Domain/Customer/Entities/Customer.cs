namespace FaceIO.Domain.Customer.Entities
{
    using Common.Entities;
    using Location.Entities;

    public class Customer : Entity
    {
        public Customer()
        {
            Locations = new List<Location>();
        }

        public string Name { get; set; } = string.Empty;

        public ICollection<Location> Locations { get; set; }
    }
}