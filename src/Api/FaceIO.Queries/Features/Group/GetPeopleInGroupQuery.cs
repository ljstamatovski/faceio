namespace FaceIO.Queries.Features.Group
{
    using Contracts.Common.Database.Context;
    using Contracts.PersonInGroup;
    using Domain.Customer.Entities;
    using Domain.Group.Entities;
    using Domain.Person.Entities;
    using Domain.PersonInGroup.Entities;
    using FaceIO.Contracts.Person;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetPeopleInGroupQuery : IRequest<IEnumerable<PersonDto>>
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public GetPeopleInGroupQuery(Guid customerUid, Guid groupUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
        }
    }

    public class GetPeopleInGroupQueryHandler : IRequestHandler<GetPeopleInGroupQuery, IEnumerable<PersonDto>>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetPeopleInGroupQueryHandler(IFaceIODbContext playerService)
        {
            _dbContext = playerService;
        }

        public async Task<IEnumerable<PersonDto>> Handle(GetPeopleInGroupQuery query, CancellationToken cancellationToken)
        {
            return await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.CustomerUid)
                          join dbGroup in _dbContext.Set<Group>().Where(x => x.DeletedOn == null
                                                                          && x.Uid == query.GroupUid)
                          on dbCustomer.Id equals dbGroup.CustomerFk
                          join dbPersonInGroup in _dbContext.Set<PersonInGroup>().Where(x => x.DeletedOn == null)
                          on dbGroup.Id equals dbPersonInGroup.GroupFk
                          join dbPerson in _dbContext.Set<Person>().Where(x => x.DeletedOn == null)
                          on dbPersonInGroup.PersonFk equals dbPerson.Id
                          select new PersonDto
                          {
                              Id = dbPerson.Id,
                              Uid = dbPerson.Uid,
                              Name = dbPerson.Name,
                              Email = dbPerson.Email,
                              Phone = dbPerson.Phone,
                              CreatedOn = dbPersonInGroup.CreatedOn
                          }).ToArrayAsync(cancellationToken);
        }
    }
}