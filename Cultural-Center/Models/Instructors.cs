using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cultural_Center.Models
{
    public class Instructors
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string EmailAddress { get; set; }
        public ICollection<Subjects> Subjects { get; set; }
    }
}