namespace FaceIO.Domain.Customer.Repositories
{
    using Customer.Entities;
    using System;
    using System.Threading.Tasks;

    public interface ICustomersRepository
    {
        /// <summary>
        /// Return customer for the provided customer uid.
        /// </summary>
        /// <param name="customerUid"> Uid of the customer. </param>
        Task<Customer> GetCustomerAsync(Guid customerUid);
    }
}