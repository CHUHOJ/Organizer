using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Organizer.Model
{
    public class Meeting
    {
        public Meeting()
        {
            Persons = new Collection<Person>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ICollection<Person> Persons { get; set; }
    }
}
