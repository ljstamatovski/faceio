namespace FaceIO.Queries.Features.Location
{
    using Domain.Location.Entities;
    using FaceIO.Contracts.Common.Database.Context;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class GetAllLocationsQuery : IRequest<IEnumerable<Location>>
    {
    }

    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, IEnumerable<Location>>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetAllLocationsQueryHandler(IFaceIODbContext playerService)
        {
            _dbContext = playerService;
        }

        public async Task<IEnumerable<Location>> Handle(GetAllLocationsQuery query, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Location>().ToArrayAsync();
        }
    }
}
