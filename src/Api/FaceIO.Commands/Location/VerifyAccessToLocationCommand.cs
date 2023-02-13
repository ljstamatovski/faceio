namespace FaceIO.Commands.Location
{
    using Amazon.Rekognition;
    using Amazon.Rekognition.Model;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.Location.Repositories;
    using MediatR;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class VerifyAccessToLocationCommand : IRequest<bool>
    {
        public Guid CustomerUid { get; }

        public Guid LocationUid { get; }

        public MemoryStream? Stream { get; }

        public VerifyAccessToLocationCommand(
            Guid customerUid,
            Guid locationUid,
            MemoryStream? stream)
        {
            CustomerUid = customerUid;
            LocationUid = locationUid;
            Stream = stream;
        }
    }

    public class VerifyAccessToLocationCommandHandler : IRequestHandler<VerifyAccessToLocationCommand, bool>
    {
        public readonly IAmazonRekognition _awsRekognition;
        private readonly ILocationsRepository _locationsRepository;

        public VerifyAccessToLocationCommandHandler(
            IAmazonRekognition awsRekognition,
            ILocationsRepository locationsRepository)
        {
            _awsRekognition = awsRekognition;
            _locationsRepository = locationsRepository;
        }

        public async Task<bool> Handle(VerifyAccessToLocationCommand command, CancellationToken cancellationToken)
        {
            Location location = await _locationsRepository.GetLocationAsync(command.CustomerUid, command.LocationUid);

            if (command.Stream is null)
            {
                throw new Exception();
            }

            var image = new Image
            {
                Bytes = command.Stream
            };

            var searchFacesByImageRequest = new SearchFacesByImageRequest()
            {
                Image = image,
                FaceMatchThreshold = 70F,
                CollectionId = location.CollectionId,
                MaxFaces = 2
            };

            SearchFacesByImageResponse searchFacesByImageResponse = await _awsRekognition.SearchFacesByImageAsync(searchFacesByImageRequest, cancellationToken);

            return searchFacesByImageResponse.FaceMatches.Any();
        }
    }
}
