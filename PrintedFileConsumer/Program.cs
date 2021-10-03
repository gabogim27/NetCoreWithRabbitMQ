using Microsoft.EntityFrameworkCore;
using System;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Source.Data;
using TFI.PrimerParcial.Source.Repository.Implementations;

namespace PrintedFileConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = GetContext();
            var repo = new Repository<FileUploadInfo>(context);
            var obj = new FileUploadInfo 
            {
                DatabaseUpdated = DateTime.Now,
                PrintDate = DateTime.Now.AddHours(-5),
                FileName = "Testing File",
                Id = Guid.NewGuid()
            };
            repo.Add(obj);
            context.SaveChanges();
            Console.WriteLine("Para terminar presione cualquier tecla");
            Console.ReadKey();
        }

        private static FileInfoDbContext GetContext()
        {
            var dbOptions = new DbContextOptionsBuilder<FileInfoDbContext>()
                .UseSqlServer("Server=WS-PF0Y4N5L;Database=FileInfoDb;Trusted_connection=true")
                .Options;

            var db = new FileInfoDbContext(dbOptions);
            return db;
        }
    }
}
