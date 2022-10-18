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

            modelBuilder.ApplyConfiguration(new CustomerMapping());
            modelBuilder.ApplyConfiguration(new GroupLocationMapping());
            modelBuilder.ApplyConfiguration(new GroupMapping());
            modelBuilder.ApplyConfiguration(new LocationMapping());
            modelBuilder.ApplyConfiguration(new PersonImageMapping());
            modelBuilder.ApplyConfiguration(new PersonInGroupMapping());
            modelBuilder.ApplyConfiguration(new PersonMapping());
        }
    }
}