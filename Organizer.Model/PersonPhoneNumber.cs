using System.ComponentModel.DataAnnotations;

namespace Organizer.Model
{
    public class PersonPhoneNumber
    {
        public int Id { get; set; }
        [Phone]
        [Required]
        public string Number { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
