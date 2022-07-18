namespace FaceIO.Queries.Features.Location
{
    using Contracts.Common.Database.Context;
    using Contracts.Common.Exceptions;
    using Contracts.Location;
    using Domain.Customer.Entities;
    using Domain.Location.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetLocationQuery : IRequest<LocationDto>
    {
        public Guid CustomerUid { get; }

        public Guid LocationUid { get; }

        public GetLocationQuery(Guid customerUid, Guid locationUid)
        {
            CustomerUid = customerUid;
            LocationUid = locationUid;
        }
    }

    public class GetLocationQueryHandler : IRequestHandler<GetLocationQuery, LocationDto>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetLocationQueryHandler(IFaceIODbContext playerService)
        {
            _dbContext = playerService;
        }

        public async Task<LocationDto> Handle(GetLocationQuery query, CancellationToken cancellationToken)
        {
            LocationDto? location = await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                                 && x.Uid == query.CustomerUid)
                                           join dbLocation in _dbContext.Set<Location>().Where(x => x.DeletedOn == null
                                                                                                 && x.Uid == query.LocationUid)
                                           on dbCustomer.Id equals dbLocation.CustomerFk
                                           select new LocationDto
                                           {
                                               Id = dbLocation.Id,
                                               Uid = dbLocation.Uid,
                                               CreatedOn = dbLocation.CreatedOn,
                                               Name = dbLocation.Name,
                                               Description = dbLocation.Description
                                           }).SingleOrDefaultAsync(cancellationToken);

            if (location is null)
            {
                throw new FaceIONotFoundException($"Location with uid {query.LocationUid} not found.");
            }

            return location;
        }
    }
}