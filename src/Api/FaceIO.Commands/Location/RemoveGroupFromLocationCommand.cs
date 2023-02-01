namespace FaceIO.Commands.Location
{
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.Location.Repositories;
    using MediatR;
    using System;
    using System.Threading.Tasks;

    public class RemoveGroupFromLocationCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public Guid LocationUid { get; }

        public RemoveGroupFromLocationCommand(Guid customerUid, Guid groupUid, Guid locationUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            LocationUid = locationUid;
        }
    }

    public class RemoveGroupFromLocationCommandHandler : IRequestHandler<RemoveGroupFromLocationCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly ILocationsRepository _locationsRepository;

        public RemoveGroupFromLocationCommandHandler(IFaceIODbContext dbContext, ILocationsRepository locationsRepository)
        {
            _dbContext = dbContext;
            _locationsRepository = locationsRepository;
        }

        public async Task<Unit> Handle(RemoveGroupFromLocationCommand request, CancellationToken cancellationToken)
        {
            Location location = await _locationsRepository.GetLocationAsync(customerUid: request.CustomerUid, locationUid: request.LocationUid);

            location.RemoveGroup(request.GroupUid);

            await _dbContext.SaveChangesAsync(cancellationToken);

            // TODO - add logic for updating collection

            return Unit.Value;
        }
    }
}