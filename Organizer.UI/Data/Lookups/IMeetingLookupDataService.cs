using Organizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organizer.UI.Data.Lookups
{
    public interface IMeetingLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetMeetingLookupAsync();
    }
}