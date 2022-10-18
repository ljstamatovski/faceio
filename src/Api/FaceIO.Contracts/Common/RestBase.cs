namespace FaceIO.Contracts.Common
{
    using System;
    using System.Text.Json.Serialization;

    public class RestBase
    {
        [JsonIgnore]
        public int Id { get; set; }

        public Guid Uid { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}