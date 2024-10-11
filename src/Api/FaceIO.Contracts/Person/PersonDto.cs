namespace FaceIO.Contracts.Person
{
    using Common;

    public class PersonDto : RestBase
    {
        public string Name { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}