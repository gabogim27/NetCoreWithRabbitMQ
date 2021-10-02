using TFI.PrimerParcial.Domain;

namespace TFI.PrimerParcial.Source.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void Update(T entity);

        FileUploadInfo GetByName(string name);
    }
}
