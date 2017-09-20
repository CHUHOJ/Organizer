using System.Collections.Generic;
using Organizer.Model;

namespace Organizer.UI.Data
{
    public interface IPersonDataService
    {
        IEnumerable<Person> GetAll();
    }
}