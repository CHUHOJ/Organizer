using Organizer.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Organizer.DataAccess
{
    public class OrganizerDbContext : DbContext
    {

        public OrganizerDbContext() : base("OrganizerDb")
        {

        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }

        public DbSet<PersonPhoneNumber> PersonPhoneNumbers { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Configurations.Add(new PersonConfiguration());
        }
    }

    //public class PersonConfiguration : EntityTypeConfiguration<Person>
    //{
    //    public PersonConfiguration()
    //    {
    //        Property(p => p.FirstName)
    //                        .IsRequired()
    //                        .HasMaxLength(50);
    //    }
    //}
}
