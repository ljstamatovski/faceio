namespace FaceIO.Commands.Location
{
    using Amazon.Rekognition;
    using Amazon.Rekognition.Model;
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.Location.Repositories;
    using FaceIO.Domain.Person.Repositories;
    using FaceIO.Domain.PersonAccessToLocation.Entities;
    using MediatR;
    using System;
    using System.Net;
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
        private readonly IPersonsRepository _personsRepository;
        private readonly IAmazonRekognition _awsRekognition;

        public RemoveGroupFromLocationCommandHandler(
            IFaceIODbContext dbContext,
            ILocationsRepository locationsRepository,
            IPersonsRepository personsRepository,
            IAmazonRekognition awsRekognition)
        {
            _dbContext = dbContext;
            _locationsRepository = locationsRepository;
            _personsRepository = personsRepository;
            _awsRekognition= awsRekognition;
        }

        public async Task<Unit> Handle(RemoveGroupFromLocationCommand request, CancellationToken cancellationToken)
        {
            Location location = await _locationsRepository.GetLocationAsync(customerUid: request.CustomerUid, locationUid: request.LocationUid);

            location.RemoveGroup(request.GroupUid);

            IReadOnlyList<PersonAccessToLocation> personsAccessesToLocation = await _personsRepository.GetPersonsAccessToLocationAsync(customerUid: request.CustomerUid,
                                                                                                                                       locationUid: request.LocationUid,
                                                                                                                                       groupUid: request.GroupUid);

            foreach (PersonAccessToLocation personAccessToLocation in personsAccessesToLocation)
            {
                personAccessToLocation.MarkAsDeleted();
            }

            var deleteFacesRequest = new DeleteFacesRequest
            {
                CollectionId = location.CollectionId,
                FaceIds = personsAccessesToLocation.Where(x => x.FaceId != null)
                                                   .Select(x => x.FaceId)
                                                   .ToList()
            };

            HttpStatusCode responseStatusCode = HttpStatusCode.OK;

            if (deleteFacesRequest.FaceIds.Any())
            {
                DeleteFacesResponse response = await _awsRekognition.DeleteFacesAsync(deleteFacesRequest, cancellationToken);
                responseStatusCode = response.HttpStatusCode;
            }

            if (responseStatusCode == HttpStatusCode.OK)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception();
            }

            return Unit.Value;
        }
    }
}