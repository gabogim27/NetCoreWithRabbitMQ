using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFI.PrimerParcial.Source.Config
{
    public class ConsumedFileConfig : IEntityTypeConfiguration<ConsumedFile>
    {
        public void Configure(EntityTypeBuilder<ConsumedFile> builder)
        {
            builder.Property(f => f.Id).IsRequired();
            builder.Property(f => f.FileName).IsRequired().HasMaxLength(100);
            builder.Property(f => f.PrintDate).IsRequired();
            builder.Property(f => f.DatabaseUpdated).IsRequired();
        }
    }
}
