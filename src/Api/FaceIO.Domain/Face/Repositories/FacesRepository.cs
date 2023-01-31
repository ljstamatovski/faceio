namespace FaceIO.Domain.Face.Repositories
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

    public class FacesRepository : Repository<Face>, IFacesRepository
    {
        public FacesRepository(IFaceIODbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Face> GetFaceAsync(Guid customerUid, Guid personUid, Guid faceUid)
        {
            Face? face = await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                      && x.Uid == customerUid)
                                join dbPerson in _dbContext.Set<Person>().Where(x => x.DeletedOn == null
                                                                                  && x.Uid == personUid)
                                on dbCustomer.Id equals dbPerson.CustomerFk
                                join dbFace in _dbContext.Set<Face>().Where(x => x.DeletedOn == null
                                                                                            && x.Uid == faceUid)
                                on dbPerson.Id equals dbFace.PersonFk
                                select dbFace).SingleOrDefaultAsync();

            if (face is null)
            {
                throw new FaceIONotFoundException($"Face with uid {faceUid} not found.");
            }

            return face;
        }
    }
}