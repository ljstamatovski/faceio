namespace FaceIO.Queries.Features.Person
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Contracts.Common.Settings;
    using FaceIO.Domain.Customer.Entities;
    using FaceIO.Domain.Person.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetPersonFaceQuery : IRequest<string>
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public GetPersonFaceQuery(Guid customerUid, Guid personUid)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
        }
    }

    public class GetPersonFaceQueryHandler : IRequestHandler<GetPersonFaceQuery, string>
    {
        private readonly IAmazonS3 _awsS3;
        private readonly IAwsSettings _awsSettings;
        private readonly IFaceIODbContext _dbContext;

        public GetPersonFaceQueryHandler(
            IFaceIODbContext dbContext,
            IAmazonS3 awsS3,
            IAwsSettings awsSettings)
        {
            _awsS3 = awsS3;
            _awsSettings = awsSettings;
            _dbContext = dbContext;
        }

        public async Task<string> Handle(GetPersonFaceQuery query, CancellationToken cancellationToken)
        {
            string? fileName = await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                             && x.Uid == query.CustomerUid)
                                      join dbPerson in _dbContext.Set<Person>().Where(x => x.DeletedOn == null
                                                                                                                  && x.Uid == query.PersonUid)
                                                                on dbCustomer.Id equals dbPerson.CustomerFk
                                      select dbPerson.FileName).SingleOrDefaultAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _awsSettings.BucketName,
                    Key = fileName,
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    Verb = HttpVerb.GET
                };

                return _awsS3.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error encountered on server. Message:'{e.Message}' when generating pre-signed URL");
                return string.Empty;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unknown error encountered on server. Message:'{e.Message}' when generating pre-signed URL");
                return string.Empty;
            }
        }
    }
}