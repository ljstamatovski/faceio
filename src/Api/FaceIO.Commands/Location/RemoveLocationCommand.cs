namespace FaceIO.Commands.Location
{
    using Amazon.Rekognition;
    using Amazon.Rekognition.Model;
    using Contracts.Common.Database.Context;
    using FaceIO.Domain.Customer.Entities;
    using FaceIO.Domain.Customer.Repositories;
    using FaceIO.Domain.Location.Entities;
    using FaceIO.Domain.Location.Repositories;
    using MediatR;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemoveLocationCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid LocationUid { get; }

        public RemoveLocationCommand(Guid customerUid, Guid locationUid)
        {
            CustomerUid = customerUid;
            LocationUid = locationUid;
        }
    }

    public class RemoveLocationCommandHandler : IRequestHandler<RemoveLocationCommand>
    {
        private readonly IFaceIODbContext _dbContext;
        private readonly ILocationsRepository _locationsRepository;
        private readonly IAmazonRekognition _awsRekognition;

        public RemoveLocationCommandHandler(
            IFaceIODbContext dbContext,
            ILocationsRepository locationsRepository,
            IAmazonRekognition awsRekognition)
        {
            _dbContext = dbContext;
            _locationsRepository = locationsRepository;
            _awsRekognition = awsRekognition;
        }

        public async Task<Unit> Handle(RemoveLocationCommand request, CancellationToken cancellationToken)
        {
            Location location = await _locationsRepository.GetLocationAsync(request.CustomerUid, request.LocationUid);

            location.MarkAsDeleted();


            var deleteCollectionRequest = new DeleteCollectionRequest()
            {
                CollectionId = location.CollectionId
            };

            DeleteCollectionResponse response = await _awsRekognition.DeleteCollectionAsync(deleteCollectionRequest, cancellationToken);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                //
            }

            return Unit.Value;
        }
    }
}