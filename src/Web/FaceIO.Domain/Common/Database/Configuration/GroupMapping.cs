namespace FaceIO.Domain.Common.Database.Configuration
{
    using Group.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class GroupMapping : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Group", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Uid).HasColumnName("Uid").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").HasColumnType("smalldatetime").IsRequired();
            builder.Property(x => x.DeletedOn).HasColumnName("DeletedOn").HasColumnType("smalldatetime");
            builder.Property(x => x.Name).HasColumnName("Name").HasColumnType("nvarchar(150)").HasMaxLength(150).IsRequired();
            builder.Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar(300)").HasMaxLength(300).IsRequired();

            builder.Property(x => x.CustomerFk).HasColumnName("CustomerFk").HasColumnType("int").IsRequired();
        }
    }
}
