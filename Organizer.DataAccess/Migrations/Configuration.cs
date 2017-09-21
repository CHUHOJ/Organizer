namespace Organizer.DataAccess.Migrations
{
    using Model;
    using System.Data.Entity.Migrations;

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
                new Person { FirstName = "Micha�", LastName = "Rusowski" },
                new Person { FirstName = "Jan", LastName = "Wi�niewski" },
                new Person { FirstName = "J�zef", LastName = "Nowak" },
                new Person { FirstName = "Witold", LastName = "Kowalski" }
            );
        }
    }
}
