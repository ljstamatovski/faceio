namespace FaceIO.Contracts.Group
{
    public class UpdateGroupRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}