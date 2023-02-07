namespace FaceIO.Commands.PersonInGroup
{
    using Amazon.Rekognition;
    using Amazon.Rekognition.Model;
    using Contracts.Common.Database.Context;
    using Domain.Group.Entities;
    using Domain.Group.Repositories;
    using Domain.Location.Entities;
    using Domain.Person.Entities;
    using Domain.Person.Repositories;
    using FaceIO.Contracts.Common.Settings;
    using FaceIO.Domain.GroupAccessToLocation.Entities;
    using FaceIO.Domain.Location.Repositories;
    using FaceIO.Domain.PersonAccessToLocation.Entities;
    using FaceIO.Domain.PersonInGroup.Entities;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddPersonInGroupCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public Guid PersonUid { get; }

        public AddPersonInGroupCommand(Guid customerUid, Guid groupUid, Guid personUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
            PersonUid = personUid;
        }
    }

    public class AddPersonInGroupCommandHandler : IRequestHandler<AddPersonInGroupCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly IGroupsRepository _groupsRepository;
        private readonly IPersonsRepository _personsRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly IAwsSettings _awsSettings;
        private readonly IAmazonRekognition _awsRekognition;

        public AddPersonInGroupCommandHandler(
            IFaceIODbContext dbContext,
            IAwsSettings awsSettings,
            IAmazonRekognition awsRekognition,
            IGroupsRepository groupsRepository,
            IPersonsRepository personsRepository,
            ILocationsRepository locationsRepository)
        {
            _dbContext = dbContext;
            _awsSettings = awsSettings;
            _awsRekognition = awsRekognition;
            _groupsRepository = groupsRepository;
            _personsRepository = personsRepository;
            _locationsRepository = locationsRepository;
        }

        public async Task<Unit> Handle(AddPersonInGroupCommand command, CancellationToken cancellationToken)
        {
            Group group = await _groupsRepository.GetGroupAsync(customerUid: command.CustomerUid, groupUid: command.GroupUid);

            Person person = await _personsRepository.GetPersonAsync(customerUid: command.CustomerUid, personUid: command.PersonUid);

            PersonInGroup personInGroup = PersonInGroup.Factory.Create(groupId: group.Id, personId: person.Id);

            _dbContext.Set<PersonInGroup>().Add(personInGroup);

            IReadOnlyList<Location> locations = await _locationsRepository.GetGroupLocationsAsync(customerUid: command.CustomerUid, groupUid: command.GroupUid);

            foreach (Location location in locations)
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
                    CollectionId = location.CollectionId,
                    DetectionAttributes = new List<string>() { "DEFAULT" }
                };

                IndexFacesResponse indexFacesResponse = await _awsRekognition.IndexFacesAsync(indexFacesRequest, cancellationToken);

                if (indexFacesResponse.FaceRecords.Count > 0)
                {
                    GroupAccessToLocation groupAccessToLocation = location.GetGroupAccessToLocation(group.Uid);

                    var personAccessToLocation = PersonAccessToLocation.Factory.Create(person, groupAccessToLocation, indexFacesResponse.FaceRecords[0].Face.FaceId);

                    _dbContext.Set<PersonAccessToLocation>().Add(personAccessToLocation);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}