using Organizer.DataAccess;
using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public class PersonDataService : IPersonDataService
    {
        private readonly Func<OrganizerDbContext> _contextCreator;

        public PersonDataService(Func<OrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<Person> GetByIdAsync(int personId)
        {
            using (OrganizerDbContext context = _contextCreator())
            {
                return await context.Persons.AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == personId);
            }
        }
    }
}
