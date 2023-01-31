namespace FaceIO.Commands.Face
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Settings;
    using Domain.Face.Entities;
    using Domain.Face.Repositories;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemoveFaceCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public Guid FaceUid { get; }

        public RemoveFaceCommand(Guid customerUid, Guid personUid, Guid faceUid)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
            FaceUid = faceUid;
        }
    }

    public class RemoveFaceCommandHandler : IRequestHandler<RemoveFaceCommand>
    {
        private readonly IAmazonS3 _awsS3;
        private readonly IAwsSettings _awsSettings;
        private readonly IFaceIODbContext _dbContext;
        private readonly IFacesRepository _facesRepository;

        public RemoveFaceCommandHandler(IAmazonS3 awsS3,
                                        IAwsSettings awsSettings,
                                        IFaceIODbContext dbContext,
                                        IFacesRepository facesRepository)
        {
            _awsS3 = awsS3;
            _awsSettings = awsSettings;
            _dbContext = dbContext;
            _facesRepository = facesRepository;
        }

        public async Task<Unit> Handle(RemoveFaceCommand command, CancellationToken cancellationToken)
        {
            Face face = await _facesRepository.GetFaceAsync(customerUid: command.CustomerUid, personUid: command.PersonUid, faceUid: command.FaceUid);

            face.MarkAsDeleted();

            try
            {
                var request = new DeleteObjectRequest
                {
                    Key = face.FileName,
                    BucketName = _awsSettings.BucketName
                };

                await _awsS3.DeleteObjectAsync(request, cancellationToken);
            }
            catch (AmazonS3Exception e)
            {
                throw new Exception($"Error encountered ***. Message:'{e?.Message}' when deleting an object", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Unknown encountered on server. Message:'{e?.Message}' when deleting an object", e);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}