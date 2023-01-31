namespace FaceIO.Contracts.Face
{
    using FaceIO.Contracts.Common;

    public class FaceDto : RestBase
    {
        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}