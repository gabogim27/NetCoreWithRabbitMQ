using System.Reflection;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class PrintDbContext : DbContext
    {
        public PrintDbContext(DbContextOptions<PrintDbContext> options) : base(options)
        {
        }

        public DbSet<ConsumedFile> ConsumedFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
