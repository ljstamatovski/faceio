namespace FaceIO.Queries.Features.Face
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using Contracts.Common.Database.Context;
    using Contracts.Common.Settings;
    using Domain.Customer.Entities;
    using Domain.Face.Entities;
    using Domain.Person.Entities;
    using FaceIO.Contracts.Face;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetPersonFacesQuery : IRequest<IEnumerable<FaceDto>>
    {
        public Guid CustomerUid { get; }

        public Guid PersonUid { get; }

        public GetPersonFacesQuery(Guid customerUid, Guid personUid)
        {
            CustomerUid = customerUid;
            PersonUid = personUid;
        }
    }

    public class GetPersonFacesQueryHandler : IRequestHandler<GetPersonFacesQuery, IEnumerable<FaceDto>>
    {
        private readonly IAmazonS3 _awsS3;
        private readonly IAwsSettings _awsSettings;
        private readonly IFaceIODbContext _dbContext;

        public GetPersonFacesQueryHandler(IAmazonS3 awsS3, IAwsSettings awsSettings, IFaceIODbContext dbContext)
        {
            _awsS3 = awsS3;
            _awsSettings = awsSettings;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FaceDto>> Handle(GetPersonFacesQuery query, CancellationToken cancellationToken)
        {
            return await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.CustomerUid)
                          join dbPerson in _dbContext.Set<Person>().Where(x => x.DeletedOn == null
                                                                            && x.Uid == query.PersonUid)
                          on dbCustomer.Id equals dbPerson.CustomerFk
                          join dbFace in _dbContext.Set<Face>().Where(x => x.DeletedOn == null)
                          on dbPerson.Id equals dbFace.PersonFk
                          select new FaceDto
                          {
                              Id = dbFace.Id,
                              Uid = dbFace.Uid,
                              CreatedOn = dbFace.CreatedOn,
                              Name = Path.GetFileName(dbFace.FileName),
                              Url = _awsS3.GetPreSignedURL(new GetPreSignedUrlRequest
                              {
                                  Key = dbFace.FileName,
                                  BucketName = _awsSettings.BucketName,
                                  Expires = DateTime.UtcNow.AddMinutes(_awsSettings.ExpirationInMinutes)
                              })
                          }).ToArrayAsync(cancellationToken: cancellationToken);
        }
    }
}