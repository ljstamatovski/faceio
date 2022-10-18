namespace FaceIO.Contracts.Customer
{
    using Common;

    public class CustomerDto : RestBase
    {
        public string Name { get; set; } = string.Empty;
    }
}