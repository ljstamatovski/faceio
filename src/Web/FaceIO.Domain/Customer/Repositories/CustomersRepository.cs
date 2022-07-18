namespace FaceIO.Domain.Customer.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Customer.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class CustomersRepository : Repository<Customer>, ICustomersRepository
    {
        public CustomersRepository(IFaceIODbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<Customer> GetCustomerAsync(Guid customerUid)
        {
            Customer? customer = await All<Customer>().SingleOrDefaultAsync(x => x.Uid == customerUid);

            if (customer is null)
            {
                throw new Exception("Customer not found.");
            }

            return customer;
        }
    }
}