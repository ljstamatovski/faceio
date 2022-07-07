namespace FaceIO.Queries.Features.Location
{
    using Contracts.Common.Database.Context;
    using Contracts.Location;
    using Domain.Customer.Entities;
    using Domain.Location.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class GetLocationsQuery : IRequest<IEnumerable<LocationDto>>
    {
        public Guid CustomerUid { get; }

        public GetLocationsQuery(Guid customerUid)
        {
            CustomerUid = customerUid;
        }
    }

    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, IEnumerable<LocationDto>>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetLocationsQueryHandler(IFaceIODbContext playerService)
        {
            _dbContext = playerService;
        }

        public async Task<IEnumerable<LocationDto>> Handle(GetLocationsQuery query, CancellationToken cancellationToken)
        {
            return await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.CustomerUid)
                          join dbLocation in _dbContext.Set<Location>().Where(x => x.DeletedOn == null)
                          on dbCustomer.Id equals dbLocation.CustomerFk
                          select new LocationDto
                          {
                              Id = dbLocation.Id,
                              Uid = dbLocation.Uid,
                              CreatedOn = dbLocation.CreatedOn,
                              Name = dbLocation.Name
                          }).ToArrayAsync(cancellationToken);
        }
    }
}
