﻿namespace FaceIO.Domain.Common.Database.Configuration
{
    using GroupLocation.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class GroupLocationMapping : IEntityTypeConfiguration<GroupLocation>
    {
        public void Configure(EntityTypeBuilder<GroupLocation> builder)
        {
            builder.ToTable("GroupLocation", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Uid).HasColumnName("Uid").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").HasColumnType("smalldatetime").IsRequired();
            builder.Property(x => x.DeletedOn).HasColumnName("DeletedOn").HasColumnType("smalldatetime");

            builder.Property(x => x.GroupFk).HasColumnName("GroupFk").HasColumnType("int").IsRequired();
            builder.Property(x => x.LocationFk).HasColumnName("LocationFk").HasColumnType("int").IsRequired();
        }
    }
}