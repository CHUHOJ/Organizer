using Organizer.Model;
using System.Threading.Tasks;

namespace Organizer.UI.Data.Repositories
{
    public interface IPersonRepository
    {
        Task<Person> GetByIdAsync(int personId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Person person);
        void Remove(Person person);
    }
}