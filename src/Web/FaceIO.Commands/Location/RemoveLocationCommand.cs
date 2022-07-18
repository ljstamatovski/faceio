namespace FaceIO.Commands.Location
{
    using Contracts.Common.Database.Context;
    using Domain.Location.Entities;
    using Domain.Location.Repositories;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemoveLocationCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid LocationUid { get; }

        public RemoveLocationCommand(Guid customerUid, Guid locationUid)
        {
            CustomerUid = customerUid;
            LocationUid = locationUid;
        }
    }

    public class RemoveLocationCommandHandler : IRequestHandler<RemoveLocationCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly ILocationsRepository _locationsRepository;

        public RemoveLocationCommandHandler(IFaceIODbContext dbContext, ILocationsRepository locationsRepository)
        {
            _dbContext = dbContext;
            _locationsRepository = locationsRepository;
        }

        public async Task<Unit> Handle(RemoveLocationCommand request, CancellationToken cancellationToken)
        {
            Location location = await _locationsRepository.GetLocationAsync(request.CustomerUid, request.LocationUid);

            location.MarkAsDeleted();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
