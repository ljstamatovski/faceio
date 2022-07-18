namespace FaceIO.Contracts.Group
{
    using Common;

    public class GroupDto : RestBase
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}