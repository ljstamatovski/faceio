namespace FaceIO.Domain.Common.Database.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PersonInGroup.Entities;

    internal class PersonInGroupMapping : IEntityTypeConfiguration<PersonInGroup>
    {
        public void Configure(EntityTypeBuilder<PersonInGroup> builder)
        {
            builder.ToTable("PersonInGroup", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Uid).HasColumnName("Uid").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").HasColumnType("smalldatetime").IsRequired();
            builder.Property(x => x.DeletedOn).HasColumnName("DeletedOn").HasColumnType("smalldatetime");

            builder.Property(x => x.GroupFk).HasColumnName("GroupFk◘").HasColumnType("int").IsRequired();
            builder.Property(x => x.PersonFk).HasColumnName("PersonFk").HasColumnType("int").IsRequired();
        }
    }
}