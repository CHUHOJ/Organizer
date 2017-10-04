using System;
using System.Threading.Tasks;
using Organizer.DataAccess;
using Organizer.Model;
using System.Data.Entity;

namespace Organizer.UI.Data.Repositories
{
    public class ProgrammingLanguageRepository : GenericRepository<ProgrammingLanguage, OrganizerDbContext>, IProgrammingLanguageRepository
    {
        public ProgrammingLanguageRepository(OrganizerDbContext context) : base(context)
        {
        }

        public async Task<bool> IsReferencesByPersonAsync(int programmingLanguageId)
        {
            return await Context.Persons.AsNoTracking()
                .AnyAsync(x => x.FavouriteLanguageId == programmingLanguageId);
        }
    }
}
