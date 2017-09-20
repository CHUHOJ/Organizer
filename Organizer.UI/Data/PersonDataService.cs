using Organizer.Model;
using System.Collections.Generic;

namespace Organizer.UI.Data
{
    public class PersonDataService : IPersonDataService
    {
        public IEnumerable<Person> GetAll()
        {
            // TODO: from db
            yield return new Person { FirstName = "Adam", LastName = "Bolski" };
            yield return new Person { FirstName = "Michał", LastName = "Rusowski" };
            yield return new Person { FirstName = "Jan", LastName = "Wiśniewski" };
            yield return new Person { FirstName = "Józef", LastName = "Nowak" };
            yield return new Person { FirstName = "Witold", LastName = "Kowalski" };
        }
    }
}
