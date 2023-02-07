using Architecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Architecture.DAL.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {

            builder.ToTable("Files");

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.FileName).HasMaxLength(255);
            builder.HasIndex(x => x.FileName).IsUnique();

            builder.Property(x => x.Path).HasMaxLength(500);

            builder.Property(x => x.CreatedAt).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.UpdatedAt).HasDefaultValue(DateTime.Now);

        }
    }
}
