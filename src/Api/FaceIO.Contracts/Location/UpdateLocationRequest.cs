namespace FaceIO.Contracts.Location
{
    public class UpdateLocationRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}