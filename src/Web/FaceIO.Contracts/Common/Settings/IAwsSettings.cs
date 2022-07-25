namespace FaceIO.Contracts.Common.Settings
{
    public interface IAwsSettings
    {
        string BucketName { get; set; }

        string AccessKey { get; set; }

        string AccessSecretKey { get; set; }
    }
}