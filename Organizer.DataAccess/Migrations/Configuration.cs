namespace Organizer.DataAccess.Migrations
{
    using Model;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OrganizerDbContext context)
        {
            context.Persons.AddOrUpdate(
                p => p.LastName,
                new Person { FirstName = "Adam", LastName = "Bolski" },
                new Person { FirstName = "Micha³", LastName = "Rusowski" },
                new Person { FirstName = "Jan", LastName = "Wiœniewski" },
                new Person { FirstName = "Józef", LastName = "Nowak" },
                new Person { FirstName = "Witold", LastName = "Kowalski" }
            );

            context.ProgrammingLanguages.AddOrUpdate(
                p => p.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "JavaScript" },
                new ProgrammingLanguage { Name = "Java" },
                new ProgrammingLanguage { Name = "PHP" },
                new ProgrammingLanguage { Name = "F#" }
                );

            context.SaveChanges();
            context.PersonPhoneNumbers.AddOrUpdate(pn => pn.Number,
                new PersonPhoneNumber { Number = "+48 515100200", PersonId = context.Persons.First().Id });
        }
    }
}
