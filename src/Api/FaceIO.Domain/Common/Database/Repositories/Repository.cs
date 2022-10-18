namespace FaceIO.Domain.Common.Database.Repositories
{
    using Common.Entities;
    using Contracts.Common.Database.Context;
    using System.Linq;

    public class Repository<TAggregate> where TAggregate : Entity
    {
        protected readonly IFaceIODbContext _dbContext;

        protected Repository(IFaceIODbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected IQueryable<TEntity> All<TEntity>() where TEntity : Entity
        {
            return _dbContext.Set<TEntity>().Where(x => x.DeletedOn == null);
        }
    }
}