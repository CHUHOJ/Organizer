using System.Threading.Tasks;

namespace Organizer.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task SaveAsync();
        bool HasChanges();
        void Add(T model);
        void Remove(T model);
    }
}
