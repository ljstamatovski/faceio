namespace FaceIO.Api.Settings
{
    using FaceIO.Contracts.Common.Settings;

    public class AwsSettings : IAwsSettings
    {
        public string BucketName { get; set; }

        public string AccessKey { get; set; }

        public string AccessSecretKey { get; set; }

        public AwsSettings(string bucketName, string accessKey, string accessSecretKey)
        {
            BucketName = bucketName;
            AccessKey = accessKey;
            AccessSecretKey = accessSecretKey;
        }
    }
}