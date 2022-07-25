namespace FaceIO.Queries.Features.PersonImage
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Settings;
    using Contracts.PersonImage;
    using Domain.Customer.Entities;
    using Domain.Person.Entities;
    using Domain.PersonImage.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetPersonImagesQuery : IRequest<IEnumerable<PersonImageDto>>
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public GetPersonImagesQuery(Guid customerUid, Guid personUid)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
        }
    }

    public class GetPersonImagesQueryHandler : IRequestHandler<GetPersonImagesQuery, IEnumerable<PersonImageDto>>
    {
        private readonly IAmazonS3 _awsS3;
        private readonly IAwsSettings _awsSettings;
        private readonly IFaceIODbContext _dbContext;

        public GetPersonImagesQueryHandler(IAmazonS3 awsS3, IAwsSettings awsSettings, IFaceIODbContext dbContext)
        {
            _awsS3 = awsS3;
            _awsSettings = awsSettings;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PersonImageDto>> Handle(GetPersonImagesQuery query, CancellationToken cancellationToken)
        {
            return await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.CustomerUid)
                          join dbPerson in _dbContext.Set<Person>().Where(x => x.DeletedOn == null
                                                                            && x.Uid == query.PersonUid)
                          on dbCustomer.Id equals dbPerson.CustomerFk
                          join dbPersonImage in _dbContext.Set<PersonImage>().Where(x => x.DeletedOn == null)
                          on dbPerson.Id equals dbPersonImage.PersonFk
                          select new PersonImageDto
                          {
                              Id = dbPersonImage.Id,
                              Uid = dbPersonImage.Uid,
                              CreatedOn = dbPersonImage.CreatedOn,
                              Name = Path.GetFileName(dbPersonImage.FileName),
                              Url = _awsS3.GetPreSignedURL(new GetPreSignedUrlRequest
                              {
                                  Key = dbPersonImage.FileName,
                                  BucketName = _awsSettings.BucketName,
                                  Expires = DateTime.UtcNow.AddMinutes(_awsSettings.ExpirationInMinutes)
                              })
                          }).ToArrayAsync(cancellationToken: cancellationToken);
        }
    }
}