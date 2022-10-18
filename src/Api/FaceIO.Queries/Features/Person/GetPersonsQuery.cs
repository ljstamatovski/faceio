namespace FaceIO.Queries.Features.Person
{
    using Contracts.Common.Database.Context;
    using Contracts.Person;
    using Domain.Customer.Entities;
    using Domain.Person.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetPersonsQuery : IRequest<IEnumerable<PersonDto>>
    {
        public Guid CustomerUid { get; }

        public GetPersonsQuery(Guid customerUid)
        {
            CustomerUid = customerUid;
        }
    }

    public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, IEnumerable<PersonDto>>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetPersonsQueryHandler(IFaceIODbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PersonDto>> Handle(GetPersonsQuery query, CancellationToken cancellationToken)
        {
            return await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.CustomerUid)
                          join dbPerson in _dbContext.Set<Person>().Where(x => x.DeletedOn == null)
                          on dbCustomer.Id equals dbPerson.CustomerFk
                          select new PersonDto
                          {
                              Id = dbPerson.Id,
                              Uid = dbPerson.Uid,
                              CreatedOn = dbPerson.CreatedOn,
                              Name = dbPerson.Name
                          }).ToArrayAsync(cancellationToken);
        }
    }
}