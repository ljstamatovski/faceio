namespace FaceIO.Commands.Person
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Contracts.Common.Settings;
    using FaceIO.Domain.Person.Entities;
    using FaceIO.Domain.Person.Repositories;
    using MediatR;
    using System;
    using System.Threading.Tasks;

    public class RemovePersonFaceCommand : IRequest
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public RemovePersonFaceCommand(Guid customerUid, Guid personUid, Guid faceUid)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
        }
    }

    public class RemovePersonFaceCommandHandler : IRequestHandler<RemovePersonFaceCommand>
    {
        private readonly IAmazonS3 _awsS3;
        private readonly IAwsSettings _awsSettings;
        private readonly IFaceIODbContext _dbContext;
        private readonly IPersonsRepository _personsRepository;

        public RemovePersonFaceCommandHandler(IAmazonS3 awsS3,
                                              IAwsSettings awsSettings,
                                              IFaceIODbContext dbContext,
                                              IPersonsRepository personsRepository)
        {
            _awsS3 = awsS3;
            _awsSettings = awsSettings;
            _dbContext = dbContext;
            _personsRepository = personsRepository;
        }

        public async Task<Unit> Handle(RemovePersonFaceCommand command, CancellationToken cancellationToken)
        {
            Person person = await _personsRepository.GetPersonAsync(customerUid: command.CustomerUid, personUid: command.PersonUid);

            try
            {
                var request = new DeleteObjectRequest
                {
                    Key = person.FileName,
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
