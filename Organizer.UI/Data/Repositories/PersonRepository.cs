using Organizer.DataAccess;
using Organizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;
using System;

namespace Organizer.UI.Data.Repositories
{
    public class PersonRepository : GenericRepository<Person, OrganizerDbContext>, IPersonRepository
    {
        public PersonRepository(OrganizerDbContext context)
            :base(context)
        {
        }

        public override async Task<Person> GetByIdAsync(int personId)
        {
            return await Context.Persons.Include(p => p.PhoneNumbers)
                .SingleOrDefaultAsync(x => x.Id == personId);
        }

        public void RemovePhoneNumber(PersonPhoneNumber model)
        {
            Context.PersonPhoneNumbers.Remove(model);
        }
    }
}
