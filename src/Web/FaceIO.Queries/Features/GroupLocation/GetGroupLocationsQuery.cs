namespace FaceIO.Queries.Features.GroupLocation
{
    using Contracts.Common.Database.Context;
    using Contracts.GroupLocation;
    using Domain.Customer.Entities;
    using Domain.Group.Entities;
    using Domain.GroupLocation.Entities;
    using Domain.Location.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetGroupLocationsQuery : IRequest<IEnumerable<GroupLocationDto>>
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public Guid LocationUid { get; }

        public GetGroupLocationsQuery(Guid customerUid, Guid groupUid, Guid locationUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            LocationUid = locationUid;
        }

        public bool Equals(GetGroupLocationsQuery other)
        {
            throw new NotImplementedException();
        }
    }

    public class GetGroupLocationsQueryHandler : IRequestHandler<GetGroupLocationsQuery, IEnumerable<GroupLocationDto>>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetGroupLocationsQueryHandler(IFaceIODbContext playerService)
        {
            _dbContext = playerService;
        }

        public async Task<IEnumerable<GroupLocationDto>> Handle(GetGroupLocationsQuery query, CancellationToken cancellationToken)
        {
            return await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.CustomerUid)
                          join dbLocation in _dbContext.Set<Location>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.LocationUid)
                          on dbCustomer.Id equals dbLocation.CustomerFk
                          join dbGroupLocation in _dbContext.Set<GroupLocation>().Where(x => x.DeletedOn == null)
                          on dbLocation.Id equals dbGroupLocation.LocationFk
                          join dbGroup in _dbContext.Set<Group>().Where(x => x.DeletedOn == null
                                                                          && x.Uid == query.GroupUid)
                          on dbGroupLocation.GroupFk equals dbGroup.Id
                          select new GroupLocationDto
                          {
                              Id = dbGroupLocation.Id,
                              Uid = dbGroupLocation.Uid,
                              CreatedOn = dbGroupLocation.CreatedOn,
                              GroupUid = dbGroup.Uid,
                              LocationUid = dbLocation.Uid
                          }).ToArrayAsync(cancellationToken);
        }
    }
}