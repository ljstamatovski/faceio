namespace FaceIO.Domain.PersonImage.Repositories
{
    using Common.Database.Repositories;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Exceptions;
    using Customer.Entities;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Person.Entities;
    using System;
    using System.Threading.Tasks;

    public class PersonImagesRepository : Repository<PersonImage>, IPersonImagesRepository
    {
        public PersonImagesRepository(IFaceIODbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PersonImage> GetPersonImageAsync(Guid customerUid, Guid personUid, Guid personImageUid)
        {
            PersonImage? personImage = await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                                    && x.Uid == customerUid)
                                              join dbPerson in _dbContext.Set<Person>().Where(x => x.DeletedOn == null
                                                                                                && x.Uid == personUid)
                                              on dbCustomer.Id equals dbPerson.CustomerFk
                                              join dbPersonImage in _dbContext.Set<PersonImage>().Where(x => x.DeletedOn == null
                                                                                                          && x.Uid == personImageUid)
                                              on dbPerson.Id equals dbPersonImage.PersonFk
                                              select dbPersonImage).SingleOrDefaultAsync();

            if (personImage is null)
            {
                throw new FaceIONotFoundException($"Person image with uid {personImageUid} not found.");
            }

            return personImage;
        }
    }
}