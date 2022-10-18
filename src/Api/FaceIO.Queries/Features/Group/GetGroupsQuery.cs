namespace FaceIO.Queries.Features.Group
{
    using Contracts.Common.Database.Context;
    using Contracts.Group;
    using Domain.Customer.Entities;
    using Domain.Group.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;

    public class GetGroupsQuery : IRequest<IEnumerable<GroupDto>>
    {
        public Guid CustomerUid { get; }

        public GetGroupsQuery(Guid customerUid)
        {
            CustomerUid = customerUid;
        }
    }

    public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, IEnumerable<GroupDto>>
    {
        private readonly IFaceIODbContext _dbContext;

        public GetGroupsQueryHandler(IFaceIODbContext playerService)
        {
            _dbContext = playerService;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetGroupsQuery query, CancellationToken cancellationToken)
        {
            return await (from dbCustomer in _dbContext.Set<Customer>().Where(x => x.DeletedOn == null
                                                                                && x.Uid == query.CustomerUid)
                          join dbGroup in _dbContext.Set<Group>().Where(x => x.DeletedOn == null)
                          on dbCustomer.Id equals dbGroup.CustomerFk
                          select new GroupDto
                          {
                              Id = dbGroup.Id,
                              Uid = dbGroup.Uid,
                              CreatedOn = dbGroup.CreatedOn,
                              Name = dbGroup.Name,
                              Description = dbGroup.Description
                          }).ToArrayAsync(cancellationToken);
        }
    }
}
