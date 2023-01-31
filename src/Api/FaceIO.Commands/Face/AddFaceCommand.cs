namespace FaceIO.Commands.Face
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using Contracts.Common;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Settings;
    using Domain.Person.Entities;
    using Domain.Person.Repositories;
    using Domain.Face.Entities;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddFaceCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public UploadImageRequest ImageRequest { get; }

        public AddFaceCommand(Guid customerUid, Guid personUid, UploadImageRequest imageRequest)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
            ImageRequest = imageRequest;
        }
    }

    public class AddFaceCommandHandler : IRequestHandler<AddFaceCommand>
    {
        private readonly IAmazonS3 _awsS3;
        private readonly IAwsSettings _awsSettings;
        private readonly IFaceIODbContext _dbContext;
        private readonly IPersonsRepository _personsRepository;

        public AddFaceCommandHandler(IAmazonS3 awsS3, IAwsSettings awsSettings, IFaceIODbContext dbContext, IPersonsRepository personsRepository)
        {
            _awsS3 = awsS3;
            _awsSettings = awsSettings;
            _dbContext = dbContext;
            _personsRepository = personsRepository;
        }

        public async Task<Unit> Handle(AddFaceCommand command, CancellationToken cancellationToken)
        {
            Person person = await _personsRepository.GetPersonAsync(command.CustomerUid, command.PersonUid);

            var request = new PutObjectRequest
            {
                BucketName = _awsSettings.BucketName,
                InputStream = command.ImageRequest.FileStream,
                ContentType = command.ImageRequest.ContentType,
                Key = $"{command.CustomerUid}/{person.Uid}/{command.ImageRequest.FileName}"
            };

            try
            {
                PutObjectResponse response = await _awsS3.PutObjectAsync(request, cancellationToken);
            }
            catch (AmazonS3Exception e)
            {
                throw new Exception($"Error encountered ***. Message:'{e?.Message}' when writing an object", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Unknown encountered on server. Message:'{e?.Message}' when writing an object", e);
            }

            var face = Face.Factory.Create(request.Key, person.Id);

            await _dbContext.Set<Face>().AddAsync(face, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
