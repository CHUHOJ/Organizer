using Organizer.DataAccess;
using Organizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;
using System;

namespace Organizer.UI.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly OrganizerDbContext _context;

        public PersonRepository(OrganizerDbContext context)
        {
            _context = context;
        }

        public void Add(Person person)
        {
            _context.Persons.Add(person);
        }

        public async Task<Person> GetByIdAsync(int personId)
        {
            return await _context.Persons.SingleOrDefaultAsync(x => x.Id == personId);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Remove(Person person)
        {
            _context.Persons.Remove(person);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
