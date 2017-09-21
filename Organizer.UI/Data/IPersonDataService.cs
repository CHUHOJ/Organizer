using System.Collections.Generic;
using Organizer.Model;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public interface IPersonDataService
    {
        Task<Person> GetByIdAsync(int personId);
    }
}