using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TFI.PrimerParcial.Domain;

namespace TFI.PrimerParcial.Source.Data
{
    public class FileInfoDbContext : DbContext
    {
        public FileInfoDbContext(DbContextOptions<FileInfoDbContext> options) : base(options)
        {
        }

        public DbSet<FileUploadInfo> FileUploadInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
