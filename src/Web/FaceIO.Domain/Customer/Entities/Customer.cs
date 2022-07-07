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

        public string Name { get; protected internal set; } = string.Empty;

        public ICollection<Location> Locations { get; protected internal set; }
    }
}