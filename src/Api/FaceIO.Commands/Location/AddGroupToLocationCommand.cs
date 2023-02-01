namespace FaceIO.Commands.Location
{
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Domain.Group.Entities;
    using FaceIO.Domain.Group.Repositories;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.Location.Repositories;
    using MediatR;
    using System;
    using System.Threading.Tasks;

    public class AddGroupToLocationCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public Guid LocationUid { get; }

        public AddGroupToLocationCommand(Guid customerUid, Guid groupUid, Guid locationUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            LocationUid = locationUid;
        }
    }

    public class AddGroupToLocationCommandHandler : IRequestHandler<AddGroupToLocationCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IGroupsRepository _groupsRepository;
        private readonly ILocationsRepository _locationsRepository;

        public AddGroupToLocationCommandHandler(IFaceIODbContext dbContext, IGroupsRepository groupsRepository, ILocationsRepository locationsRepository)
        {
            _dbContext = dbContext;
            _groupsRepository = groupsRepository;
            _locationsRepository = locationsRepository;
        }

        public async Task<Unit> Handle(AddGroupToLocationCommand request, CancellationToken cancellationToken)
        {
            Group group = await _groupsRepository.GetGroupAsync(customerUid: request.CustomerUid, groupUid: request.GroupUid);

            Location location = await _locationsRepository.GetLocationAsync(customerUid: request.CustomerUid, locationUid: request.LocationUid);

            location.AddGroup(group.Id);

            await _dbContext.SaveChangesAsync(cancellationToken);

            // TODO - add logic

            return Unit.Value;
        }
    }
}