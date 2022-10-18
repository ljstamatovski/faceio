namespace FaceIO.Commands.GroupLocation
{
    using Contracts.Common.Database.Context;
    using Domain.GroupLocation.Entities;
    using Domain.GroupLocation.Repositories;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemoveLocationFromGroupCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public Guid LocationUid { get; }

        public Guid GroupLocationUid { get; }

        public RemoveLocationFromGroupCommand(Guid customerUid, Guid groupUid, Guid locationUid, Guid groupLocationUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            LocationUid = locationUid;
            GroupLocationUid = groupLocationUid;
        }
    }

    public class RemoveLocationFromGroupCommandHandler : IRequestHandler<RemoveLocationFromGroupCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IGroupLocationsRepository _groupLocationsRepository;

        public RemoveLocationFromGroupCommandHandler(IFaceIODbContext dbContext, IGroupLocationsRepository groupLocationsRepository)
        {
            _dbContext = dbContext;
            _groupLocationsRepository = groupLocationsRepository;
        }

        public async Task<Unit> Handle(RemoveLocationFromGroupCommand request, CancellationToken cancellationToken)
        {
            GroupLocation groupLocation = await _groupLocationsRepository.GetGroupLocationAsync(customerUid: request.CustomerUid,
                                                                                                groupUid: request.GroupUid,
                                                                                                locationUid: request.LocationUid,
                                                                                                groupLocationUid: request.GroupLocationUid);

            groupLocation.MarkAsDeleted();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}