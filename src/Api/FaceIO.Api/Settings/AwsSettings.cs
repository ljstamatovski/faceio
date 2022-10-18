namespace FaceIO.Api.Settings
{
    using FaceIO.Contracts.Common.Settings;

    public class AwsSettings : IAwsSettings
    {
        public string BucketName { get; set; } = string.Empty;

        public string AccessKey { get; set; } = string.Empty;

        public string AccessSecretKey { get; set; } = string.Empty;

        public int ExpirationInMinutes { get; set; }
    }
}