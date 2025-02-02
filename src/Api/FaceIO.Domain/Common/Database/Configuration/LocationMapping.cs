﻿namespace FaceIO.Domain.Common.Database.Configuration
{
    using Location.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LocationMapping : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Uid).HasColumnName("Uid").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").HasColumnType("smalldatetime").IsRequired();
            builder.Property(x => x.DeletedOn).HasColumnName("DeletedOn").HasColumnType("smalldatetime");
            builder.Property(x => x.Name).HasColumnName("Name").HasColumnType("nvarchar(150)").HasMaxLength(150).IsRequired();
            builder.Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar(300)").HasMaxLength(300);
            builder.Property(x => x.Address).HasColumnName("Address").HasColumnType("nvarchar(500)").HasMaxLength(300);
            builder.Property(x => x.CollectionId).HasColumnName("CollectionId").HasColumnType("nvarchar(75)").HasMaxLength(75).IsRequired();
            builder.Property(x => x.Latitude).HasColumnName("Latitude").HasColumnType("decimal(9,6)");
            builder.Property(x => x.Longitude).HasColumnName("Longitude").HasColumnType("decimal(9,6)");

            builder.Property(x => x.CustomerFk).HasColumnName("CustomerFk").HasColumnType("int").IsRequired();
        }
    }
}