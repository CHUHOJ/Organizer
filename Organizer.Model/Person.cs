using System.ComponentModel.DataAnnotations;

namespace Organizer.Model
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public int? FavouriteLanguageId { get; set; }

        public ProgrammingLanguage FavouriteLanguage { get; set; }
    }
}
