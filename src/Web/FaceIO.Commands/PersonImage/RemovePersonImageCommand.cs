namespace FaceIO.Commands.PersonImage
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using Contracts.Common.Database.Context;
    using Domain.PersonImage.Entities;
    using Domain.PersonImage.Repositories;
    using Contracts.Common.Settings;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemovePersonImageCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public Guid PersonImageUid { get; }

        public RemovePersonImageCommand(Guid customerUid, Guid personUid, Guid personImageUid)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
            PersonImageUid = personImageUid;
        }
    }

    public class RemovePersonImageCommandHandler : IRequestHandler<RemovePersonImageCommand>
    {
        private readonly IAmazonS3 _awsS3;
        private readonly IAwsSettings _awsSettings;
        private readonly IFaceIODbContext _dbContext;
        private readonly IPersonImagesRepository _personImagesRepository;

        public RemovePersonImageCommandHandler(IAmazonS3 awsS3,
                                               IAwsSettings awsSettings,
                                               IFaceIODbContext dbContext,
                                               IPersonImagesRepository personImagesRepository)
        {
            _awsS3 = awsS3;
            _awsSettings = awsSettings;
            _dbContext = dbContext;
            _personImagesRepository = personImagesRepository;
        }

        public async Task<Unit> Handle(RemovePersonImageCommand command, CancellationToken cancellationToken)
        {
            PersonImage personImage = await _personImagesRepository.GetPersonImageAsync(customerUid: command.CustomerUid, personUid: command.PersonUid, personImageUid: command.PersonImageUid);

            personImage.MarkAsDeleted();

            try
            {
                var request = new DeleteObjectRequest
                {
                    Key = personImage.FileName,
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