using Organizer.Model;
using System.Threading.Tasks;

namespace Organizer.UI.Data.Repositories
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        void RemovePhoneNumber(PersonPhoneNumber model);

        Task<bool> HasMeetingsAsync(int personId);
    }
}