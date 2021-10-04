using Entities;

namespace Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        ConsumedFile GetByName(string name);
    }
}
