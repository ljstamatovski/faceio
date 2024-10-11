namespace FaceIO.Queries.Features.Person
{
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Contracts.Group;
    using FaceIO.Domain.Customer.Entities;
    using FaceIO.Domain.Group.Entities;
    using FaceIO.Domain.Person.Entities;
    using FaceIO.Domain.PersonInGroup.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetPersonGroupsQuery : IRequest<IEnumerable<GroupDto>>
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public GetPersonGroupsQuery(Guid customerUid, Guid personUid)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
        }
    }

    public class GetPersonGroupsQueryHandler : IRequestHandler<GetPersonGroupsQuery, IEnumerable<GroupDto>>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetPersonGroupsQueryHandler(IFaceIODbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetPersonGroupsQuery query, CancellationToken cancellationToken)
        {
            return await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.CustomerUid)
                          join dbPerson in _dbContext.Set<Person>().Where(x => x.DeletedOn == null
                          && x.Uid == query.PersonUid)
                          on dbCustomer.Id equals dbPerson.CustomerFk
                          join dbPersonInGroup in _dbContext.Set<PersonInGroup>().Where(x => x.DeletedOn == null)
                          on dbPerson.Id equals dbPersonInGroup.PersonFk
                          join dbGroup in _dbContext.Set<Group>().Where(x => x.DeletedOn == null)
                          on dbPersonInGroup.GroupFk equals dbGroup.Id
                          select new GroupDto
                          {
                              Id = dbGroup.Id,
                              Uid = dbGroup.Uid,
                              CreatedOn = dbGroup.CreatedOn,
                              Name = dbGroup.Name,
                              Description = dbGroup.Description
                          }).ToArrayAsync(cancellationToken);
        }
    }
}