using System.Collections.Generic;
using System.Linq;
using Entities;
using Repository.Interfaces;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PrintDbContext context;

        public Repository(PrintDbContext context)
        {
            this.context = context;
        }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public ConsumedFile GetByName(string name)
        {
            return context.ConsumedFiles.Where(x => x.FileName == name).FirstOrDefault();
        }

        public List<ConsumedFile> GetList(string name)
        {
            return context.ConsumedFiles.ToList();
        }
    }
}
