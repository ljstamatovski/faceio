namespace FaceIO.Contracts.PersonImage
{
    using FaceIO.Contracts.Common;

    public class PersonImageDto : RestBase
    {
        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}