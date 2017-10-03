using System.Threading.Tasks;
using Organizer.Model;
using System.Collections.Generic;

namespace Organizer.UI.Data.Repositories
{
    public interface IMeetingRepository : IGenericRepository<Meeting>
    {
        Task<List<Person>> GetAllPersonsAsync();
    }
}