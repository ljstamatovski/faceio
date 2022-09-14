namespace FaceIO.Queries.Features.Group
{
    using Contracts.Common.Database.Context;
    using Contracts.PersonInGroup;
    using Domain.Customer.Entities;
    using Domain.Group.Entities;
    using Domain.Person.Entities;
    using Domain.PersonInGroup.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetPersonsInGroupQuery : IRequest<IEnumerable<PersonInGroupDto>>
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public GetPersonsInGroupQuery(Guid customerUid, Guid groupUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
        }
    }

    public class GetPersonsInGroupQueryHandler : IRequestHandler<GetPersonsInGroupQuery, IEnumerable<PersonInGroupDto>>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetPersonsInGroupQueryHandler(IFaceIODbContext playerService)
        {
            _dbContext = playerService;
        }

        public async Task<IEnumerable<PersonInGroupDto>> Handle(GetPersonsInGroupQuery query, CancellationToken cancellationToken)
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
                          select new PersonInGroupDto
                          {
                              Id = dbPersonInGroup.Id,
                              Uid = dbPersonInGroup.Uid,
                              PersonName = dbPerson.Name,
                              CreatedOn = dbPersonInGroup.CreatedOn
                          }).ToArrayAsync(cancellationToken);
        }
    }
}
