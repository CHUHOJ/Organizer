using Organizer.Model;
using System.Threading.Tasks;

namespace Organizer.UI.Data.Repositories
{
    public interface IProgrammingLanguageRepository : IGenericRepository<ProgrammingLanguage>
    {
        Task<bool> IsReferencesByPersonAsync(int programmingLanguageId);
    }
}
