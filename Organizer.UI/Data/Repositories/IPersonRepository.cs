using Organizer.Model;

namespace Organizer.UI.Data.Repositories
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        void RemovePhoneNumber(PersonPhoneNumber model);
    }
}