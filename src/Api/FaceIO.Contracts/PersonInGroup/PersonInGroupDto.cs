namespace FaceIO.Contracts.PersonInGroup
{
    using FaceIO.Contracts.Common;

    public class PersonInGroupDto : RestBase
    {
        public string PersonName { get; set; } = string.Empty;
    }
}