using System.Collections.Generic;
using System.Threading.Tasks;
using Organizer.Model;

namespace Organizer.UI.Data
{
    public interface IPersonLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetPersonLookupAsync();
    }
}