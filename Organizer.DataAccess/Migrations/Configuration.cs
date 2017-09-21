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
                new Person { FirstName = "Micha³", LastName = "Rusowski" },
                new Person { FirstName = "Jan", LastName = "Wiœniewski" },
                new Person { FirstName = "Józef", LastName = "Nowak" },
                new Person { FirstName = "Witold", LastName = "Kowalski" }
            );
        }
    }
}
