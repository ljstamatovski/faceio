namespace FaceIO.Queries.Features.Group
{
    using Contracts.Common.Database.Context;
    using Contracts.Common.Exceptions;
    using Contracts.Group;
    using Domain.Customer.Entities;
    using Domain.Group.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class GetGroupQuery : IRequest<GroupDto>
    {
        public Guid CustomerUid { get; }

        public Guid GroupUid { get; }

        public GetGroupQuery(Guid customerUid, Guid groupUid)
        {
            CustomerUid = customerUid;
            GroupUid = groupUid;
        }
    }

    public class GetGroupQueryHandler : IRequestHandler<GetGroupQuery, GroupDto>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetGroupQueryHandler(IFaceIODbContext playerService)
        {
            _dbContext = playerService;
        }

        public async Task<GroupDto> Handle(GetGroupQuery query, CancellationToken cancellationToken)
        {
            GroupDto? group = await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                           && x.Uid == query.CustomerUid)
                                     join dbGroup in _dbContext.Set<Group>().Where(x => x.DeletedOn == null
                                                                                     && x.Uid == query.GroupUid)
                                     on dbCustomer.Id equals dbGroup.CustomerFk
                                     select new GroupDto
                                     {
                                         Id = dbGroup.Id,
                                         Uid = dbGroup.Uid,
                                         CreatedOn = dbGroup.CreatedOn,
                                         Name = dbGroup.Name,
                                         Description = dbGroup.Description
                                     }).SingleOrDefaultAsync(cancellationToken);

            if (group is null)
            {
                throw new FaceIONotFoundException($"Group with uid {query.GroupUid} not found.");
            }

            return group;
        }
    }
}