using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFI.PrimerParcial.Domain;

namespace TFI.PrimerParcial.Source.Config
{
    public class FileUploadConfiguration : IEntityTypeConfiguration<FileUploadInfo>
    {
        public void Configure(EntityTypeBuilder<FileUploadInfo> builder)
        {
            builder.Property(f => f.Id).IsRequired();
            builder.Property(f => f.FileName).IsRequired().HasMaxLength(100);
            builder.Property(f => f.PrintDate).IsRequired();
            builder.Property(f => f.DatabaseUpdated).IsRequired();
        }
    }
}
