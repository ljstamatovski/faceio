namespace FaceIO.Commands.Location
{
    using Amazon.Rekognition;
    using Amazon.Rekognition.Model;
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Contracts.Common.Settings;
    using FaceIO.Domain.Group.Entities;
    using FaceIO.Domain.Group.Repositories;
    using FaceIO.Domain.GroupAccessToLocation.Entities;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.Location.Repositories;
    using FaceIO.Domain.Person.Entities;
    using FaceIO.Domain.Person.Repositories;
    using FaceIO.Domain.PersonAccessToLocation.Entities;
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
        private readonly IPersonsRepository _personsRepository;
        private readonly IAwsSettings _awsSettings;
        private readonly IAmazonRekognition _awsRekognition;

        public AddGroupToLocationCommandHandler(
            IFaceIODbContext dbContext,
            IGroupsRepository groupsRepository,
            ILocationsRepository locationsRepository,
            IPersonsRepository personsRepository,
            IAwsSettings awsSettings,
            IAmazonRekognition awsRekognition)
        {
            _dbContext = dbContext;
            _groupsRepository = groupsRepository;
            _locationsRepository = locationsRepository;
            _personsRepository = personsRepository;
            _awsSettings= awsSettings;
            _awsRekognition = awsRekognition;
        }

        public async Task<Unit> Handle(AddGroupToLocationCommand command, CancellationToken cancellationToken)
        {
            Group group = await _groupsRepository.GetGroupAsync(customerUid: command.CustomerUid, groupUid: command.GroupUid);

            Location location = await _locationsRepository.GetLocationAsync(customerUid: command.CustomerUid, locationUid: command.LocationUid);

            GroupAccessToLocation groupAccessToLocation = location.AddGroup(group.Id);

            IReadOnlyList<Person> personsInGroup = await _personsRepository.GetPersonsInGroupAsync(command.CustomerUid, command.GroupUid);

            foreach (Person person in personsInGroup.Where(x => x.FileName != null))
            {
                var indexFacesRequest = new IndexFacesRequest()
                {
                    Image = new Image()
                    {
                        S3Object = new S3Object()
                        {
                            Bucket = _awsSettings.BucketName,
                            Name = person.FileName
                        }
                    },
                    MaxFaces = 1,
                    ExternalImageId = person.FileName,
                    CollectionId = location.CollectionId,
                    DetectionAttributes = new List<string>() { "DEFAULT" }
                };

                IndexFacesResponse indexFacesResponse = await _awsRekognition.IndexFacesAsync(indexFacesRequest, cancellationToken);

                if (indexFacesResponse.FaceRecords.Count > 0)
                {
                    var personAccessToLocation = PersonAccessToLocation.Factory.Create(person, groupAccessToLocation, indexFacesResponse.FaceRecords[0].Face.FaceId);

                    _dbContext.Set<PersonAccessToLocation>().Add(personAccessToLocation);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}