namespace FaceIO.Domain.Common.Database.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Person.Entities;

    public class PersonMapping : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Uid).HasColumnName("Uid").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").HasColumnType("smalldatetime").IsRequired();
            builder.Property(x => x.DeletedOn).HasColumnName("DeletedOn").HasColumnType("smalldatetime");
            builder.Property(x => x.Name).HasColumnName("Name").HasColumnType("nvarchar(150)").HasMaxLength(150).IsRequired();
            builder.Property(x => x.FileName).HasColumnName("FileName").HasColumnType("nvarchar(500)").HasMaxLength(500);
            builder.Property(x => x.Email).HasColumnName("Email").HasColumnType("nvarchar(150)").HasMaxLength(150);
            builder.Property(x => x.Phone).HasColumnName("Phone").HasColumnType("nvarchar(20)").HasMaxLength(20);

            builder.Property(x => x.CustomerFk).HasColumnName("CustomerFk").HasColumnType("int").IsRequired();
        }
    }
}