namespace FaceIO.Domain.Common.Database.Configuration
{
    using Face.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FaceMapping : IEntityTypeConfiguration<Face>
    {
        public void Configure(EntityTypeBuilder<Face> builder)
        {
            builder.ToTable("Face", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Uid).HasColumnName("Uid").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").HasColumnType("smalldatetime").IsRequired();
            builder.Property(x => x.DeletedOn).HasColumnName("DeletedOn").HasColumnType("smalldatetime");
            builder.Property(x => x.FileName).HasColumnName("FileName").HasColumnType("nvarchar(500)").HasMaxLength(500).IsRequired();

            builder.Property(x => x.PersonFk).HasColumnName("PersonFk").HasColumnType("int").IsRequired();
        }
    }
}