using System.Collections.Generic;
using System.Threading.Tasks;
using Organizer.Model;

namespace Organizer.UI.Data.Lookups
{
    public interface IProgrammingLanguageDataService
    {
        Task<IEnumerable<LookupItem>> GetProgrammingLanguageAsync();
    }
}