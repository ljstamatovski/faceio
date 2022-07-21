namespace FaceIO.Commands.GroupLocation
{
    using Contracts.Common.Database.Context;
    using Domain.Group.Entities;
    using Domain.Group.Repositories;
    using Domain.GroupLocation.Entities;
    using Domain.Location.Entities;
    using Domain.Location.Repositories;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddLocationToGroupCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public Guid LocationUid { get; }

        public AddLocationToGroupCommand(Guid customerUid, Guid groupUid, Guid locationUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            LocationUid = locationUid;
        }
    }

    public class AddLocationToGroupCommandHandler : IRequestHandler<AddLocationToGroupCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IGroupsRepository _groupsRepository;
        private readonly ILocationsRepository _locationsRepository;

        public AddLocationToGroupCommandHandler(IFaceIODbContext dbContext, IGroupsRepository groupsRepository, ILocationsRepository locationsRepository)
        {
            _dbContext = dbContext;
            _groupsRepository = groupsRepository;
            _locationsRepository = locationsRepository;
        }

        public async Task<Unit> Handle(AddLocationToGroupCommand request, CancellationToken cancellationToken)
        {
            Group group = await _groupsRepository.GetGroupAsync(customerUid: request.CustomerUid, groupUid: request.GroupUid);

            Location location = await _locationsRepository.GetLocationAsync(customerUid: request.CustomerUid, locationUid: request.LocationUid);

            var groupLocation = GroupLocation.Factory.Create(groupFk: group.Id, locationFk: location.Id);

            await _dbContext.Set<GroupLocation>().AddAsync(groupLocation);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
