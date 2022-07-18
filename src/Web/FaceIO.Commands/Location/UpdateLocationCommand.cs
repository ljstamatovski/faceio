namespace FaceIO.Commands.Location
{
    using Domain.Location.Entities;
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Domain.Location.Repositories;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateLocationCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid LocationUid { get; }

        public string Name { get; }

        public string? Description { get; }

        public UpdateLocationCommand(Guid customerUid, Guid locationUid, string name, string? description)
        {
            CustomerUid = customerUid;
            LocationUid = locationUid;
            Name = name;
            Description = description;
        }
    }

    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly ILocationsRepository _locationsRepository;

        public UpdateLocationCommandHandler(IFaceIODbContext dbContext, ILocationsRepository locationsRepository)
        {
            _dbContext = dbContext;
            _locationsRepository = locationsRepository;
        }

        public async Task<Unit> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            Location location = await _locationsRepository.GetLocationAsync(request.CustomerUid, request.LocationUid);

            location.SetName(request.Name)
                    .SetDescription(request.Description);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}