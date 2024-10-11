namespace FaceIO.Queries.Features.Person
{
    using Contracts.Common.Database.Context;
    using Contracts.Person;
    using Domain.Customer.Entities;
    using Domain.Person.Entities;
    using FaceIO.Contracts.Common.Exceptions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetPersonQuery : IRequest<PersonDto>
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public GetPersonQuery(Guid customerUid, Guid personUid)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
        }
    }

    public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, PersonDto>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetPersonQueryHandler(IFaceIODbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PersonDto> Handle(GetPersonQuery query, CancellationToken cancellationToken)
        {
            PersonDto? person = await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                             && x.Uid == query.CustomerUid)
                                       join dbPerson in _dbContext.Set<Person>().Where(x => x.DeletedOn == null
                                                                                         && x.Uid == query.PersonUid)
                                       on dbCustomer.Id equals dbPerson.CustomerFk
                                       select new PersonDto
                                       {
                                           Id = dbPerson.Id,
                                           Uid = dbPerson.Uid,
                                           CreatedOn = dbPerson.CreatedOn,
                                           Name = dbPerson.Name,
                                           Email = dbPerson.Email,
                                           Phone = dbPerson.Phone
                                       }).SingleOrDefaultAsync(cancellationToken);

            if (person is null)
            {
                throw new FaceIONotFoundException($"Person with uid {query.PersonUid} not found.");
            }

            return person;
        }
    }
}
