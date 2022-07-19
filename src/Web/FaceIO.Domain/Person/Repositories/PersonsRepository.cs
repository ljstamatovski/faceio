namespace FaceIO.Domain.Person.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Customer.Entities;
    using Entities;
    using FaceIO.Contracts.Common.Exceptions;
    using Microsoft.EntityFrameworkCore;

    public class PersonsRepository : Repository<Person>, IPersonsRepository
    {
        public PersonsRepository(IFaceIODbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Person> GetPersonAsync(Guid customerUid, Guid personUid)
        {
            Person? person = await (from dbPerson in All<Person>().Where(x => x.DeletedOn == null
                                                                           && x.Uid == personUid)
                                    join dbCustomer in All<Customer>().Where(x => x.DeletedOn == null
                                                                               && x.Uid == customerUid)
                                    on dbPerson.CustomerFk equals dbCustomer.Id
                                    select dbPerson).SingleOrDefaultAsync();

            if (person is null)
            {
                throw new FaceIONotFoundException($"Person with uid {personUid} not found.");
            }

            return person;
        }
    }
}