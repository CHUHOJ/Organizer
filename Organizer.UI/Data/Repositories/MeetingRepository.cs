using Organizer.DataAccess;
using Organizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;
namespace Organizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, OrganizerDbContext>, IMeetingRepository
    {
        public MeetingRepository(OrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings
                .Include(x => x.Persons)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
