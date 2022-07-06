namespace FaceIO.Domain.Common.Database.Context
{
    using Configuration;
    using Contracts.Common.Database.Context;
    using Microsoft.EntityFrameworkCore;

    public class FaceIODbContext : DbContext, IFaceIODbContext
    {
        public FaceIODbContext(DbContextOptions<FaceIODbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LocationMapping());
        }
    }
}