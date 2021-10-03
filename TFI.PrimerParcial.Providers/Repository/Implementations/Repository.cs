using System.Linq;
using Microsoft.EntityFrameworkCore;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Source.Data;
using TFI.PrimerParcial.Source.Repository.Interfaces;

namespace TFI.PrimerParcial.Source.Repository.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FileInfoDbContext context;

        public Repository(FileInfoDbContext context)
        {
            this.context = context;
        }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            context.Set<T>().Attach(entity);

            context.Entry(entity).State = EntityState.Modified;
        }

        public FileUploadInfo GetByName(string name)
        {
            return context.FileUploadInfo.Where(x => x.FileName == name).FirstOrDefault();
        }
    }
}
