namespace FaceIO.Contracts.Common
{
    public class UploadImageRequest
    {
        public string ContentType { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public Stream? FileStream { get; set; }
    }
}