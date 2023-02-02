namespace FaceIO.Commands.Person
{
    using Amazon.Rekognition;
    using Amazon.Rekognition.Model;
    using Amazon.S3;
    using Amazon.S3.Model;
    using FaceIO.Contracts.Common;
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Contracts.Common.Settings;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.Location.Repositories;
    using FaceIO.Domain.Person.Entities;
    using FaceIO.Domain.Person.Repositories;
    using MediatR;
    using System;
    using System.Threading.Tasks;

    public class AddPersonFaceCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public UploadImageRequest ImageRequest { get; }

        public AddPersonFaceCommand(Guid customerUid, Guid personUid, UploadImageRequest imageRequest)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
            ImageRequest = imageRequest;
        }
    }

    public class AddPersonFaceCommandHandler : IRequestHandler<AddPersonFaceCommand>
    {
        private readonly IAmazonS3 _awsS3;
        private readonly IAmazonRekognition _awsRekognition;
        private readonly IAwsSettings _awsSettings;
        private readonly IFaceIODbContext _dbContext;
        private readonly IPersonsRepository _personsRepository;
        private readonly ILocationsRepository _locationsRepository;

        public AddPersonFaceCommandHandler(
            IAmazonS3 awsS3,
            IAmazonRekognition awsRekognition,
            IAwsSettings awsSettings,
            IFaceIODbContext dbContext,
            IPersonsRepository personsRepository,
            ILocationsRepository locationsRepository)
        {
            _awsS3 = awsS3;
            _awsRekognition = awsRekognition;
            _awsSettings = awsSettings;
            _dbContext = dbContext;
            _personsRepository = personsRepository;
            _locationsRepository = locationsRepository;
        }

        public async Task<Unit> Handle(AddPersonFaceCommand command, CancellationToken cancellationToken)
        {
            Person person = await _personsRepository.GetPersonAsync(command.CustomerUid, command.PersonUid);

            person.SetFileName(customerUid: command.CustomerUid, fileName: command.ImageRequest.FileName);

            var request = new PutObjectRequest
            {
                BucketName = _awsSettings.BucketName,
                InputStream = command.ImageRequest.FileStream,
                ContentType = command.ImageRequest.ContentType,
                Key = person.FileName
            };

            PutObjectResponse response = await _awsS3.PutObjectAsync(request, cancellationToken);

            IReadOnlyList<Location> locations = await _locationsRepository.GetPersonLocationsAsync(command.CustomerUid, command.PersonUid);

            foreach (Location location in locations)
            {
                var indexFacesRequest = new IndexFacesRequest()
                {
                    Image = null,
                    CollectionId = location.CollectionId,
                    ExternalImageId = person.FileName,
                    DetectionAttributes = new List<string>() { "DEFAULT" }
                };

                IndexFacesResponse indexFacesResponse = await _awsRekognition.IndexFacesAsync(indexFacesRequest);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
