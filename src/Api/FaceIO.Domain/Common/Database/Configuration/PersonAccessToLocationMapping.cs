namespace FaceIO.Domain.Common.Database.Configuration
{
    using PersonAccessToLocation.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class PersonAccessToLocationMapping : IEntityTypeConfiguration<PersonAccessToLocation>
    {
        public void Configure(EntityTypeBuilder<PersonAccessToLocation> builder)
        {
            builder.ToTable("PersonAccessToLocation", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Uid).HasColumnName("Uid").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").HasColumnType("smalldatetime").IsRequired();
            builder.Property(x => x.DeletedOn).HasColumnName("DeletedOn").HasColumnType("smalldatetime");

            builder.Property(x => x.PersonFk).HasColumnName("PersonFk").HasColumnType("int").IsRequired();
            builder.Property(x => x.GroupAccessToLocationFk).HasColumnName("GroupAccessToLocationFk").HasColumnType("int").IsRequired();

            builder.Property(x => x.FaceId).HasColumnName("FaceId").HasColumnType("nvarchar(50)").HasMaxLength(50);
        }
    }
}