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

        public static class Factory
        {
            public static Customer Create(string name)
            {
                return new Customer
                {
                    Uid = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    Name = name
                };
            }
        }
    }
}