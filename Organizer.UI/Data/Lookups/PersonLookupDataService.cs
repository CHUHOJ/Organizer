using Organizer.DataAccess;
using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Data.Lookups
{
    public class PersonLookupDataService : IPersonLookupDataService
    {
        private readonly Func<OrganizerDbContext> _contextCreator;

        public PersonLookupDataService(Func<OrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetPersonLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Persons.AsNoTracking()
                    .Select(x => 
                    new LookupItem
                    {
                        Id = x.Id,
                        DisplayMember = x.FirstName + " " + x.LastName
                    })
                    .ToListAsync();
            }
        }
    }
}
