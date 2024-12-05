namespace FaceIO.Queries.Features.Location
{
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Contracts.Group;
    using FaceIO.Domain.Customer.Entities;
    using FaceIO.Domain.Group.Entities;
    using FaceIO.Domain.GroupAccessToLocation.Entities;
    using FaceIO.Domain.Location.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class GetGroupsWithAccessToLocationQuery : IRequest<IEnumerable<GroupDto>>
    {
        public Guid CustomerUid { get; }

        public Guid LocationUid { get; }

        public GetGroupsWithAccessToLocationQuery(Guid customerUid, Guid locationUid)
        {
            CustomerUid = customerUid;
            LocationUid = locationUid;
        }
    }

    public class GetGroupsWithAccessToLocationQueryHandler : IRequestHandler<GetGroupsWithAccessToLocationQuery, IEnumerable<GroupDto>>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetGroupsWithAccessToLocationQueryHandler(IFaceIODbContext playerService)
        {
            _dbContext = playerService;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetGroupsWithAccessToLocationQuery query, CancellationToken cancellationToken)
        {
            return await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                                 && x.Uid == query.CustomerUid)
                          join dbLocation in _dbContext.Set<Location>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.LocationUid)
                          on dbCustomer.Id equals dbLocation.CustomerFk
                          join dbGroupAccessToLocation in _dbContext.Set<GroupAccessToLocation>().Where(x => x.DeletedOn == null)
                          on dbLocation.Id equals dbGroupAccessToLocation.LocationFk
                          join dbGroup in _dbContext.Set<Group>().Where(x => x.DeletedOn == null)
                          on dbGroupAccessToLocation.GroupFk equals dbGroup.Id
                          select new GroupDto
                          {
                              Id = dbGroup.Id,
                              Uid = dbGroup.Uid,
                              CreatedOn = dbGroupAccessToLocation.CreatedOn,
                              Name = dbGroup.Name,
                              Description = dbGroup.Description
                          }).ToListAsync(cancellationToken);
        }
    }
}