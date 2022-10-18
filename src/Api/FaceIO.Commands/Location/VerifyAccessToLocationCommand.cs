namespace FaceIO.Commands.Location
{
    using FaceIO.Contracts.Common;
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.Location.Repositories;
    using MediatR;

    public class VerifyAccessToLocationCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid LocationUid { get; }

        public VerifyImageRequest ImageRequest { get; }

        public VerifyAccessToLocationCommand(Guid customerUid, Guid locationUid, VerifyImageRequest imageRequest)
        {
            CustomerUid = customerUid;
            LocationUid = locationUid;
            ImageRequest = imageRequest;
        }
    }

    public class VerifyAccessToLocationCommandHandler : IRequestHandler<VerifyAccessToLocationCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly ILocationsRepository _locationsRepository;

        public VerifyAccessToLocationCommandHandler(IFaceIODbContext dbContext, ILocationsRepository locationsRepository)
        {
            _dbContext = dbContext;
            _locationsRepository = locationsRepository;
        }

        public async Task<Unit> Handle(VerifyAccessToLocationCommand command, CancellationToken cancellationToken)
        {
            var c = await _locationsRepository.GetGroupsWithLocationAccessAsync(customerUid: command.CustomerUid, locationUid: command.LocationUid);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}