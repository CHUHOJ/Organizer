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

        public async Task<List<Person>> GetAllAsync()
        {
            using (OrganizerDbContext context = _contextCreator())
            {
                var friends= await context.Persons.AsNoTracking().ToListAsync();
                //await Task.Delay(5000);

                return friends;
            }
        }
    }
}
