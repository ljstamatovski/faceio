namespace FaceIO.Contracts.Location
{
    using Common;

    public class LocationDto : RestBase
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}