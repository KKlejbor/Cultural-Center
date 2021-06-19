using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cultural_Center.Models
{
    public class Instructors
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(15)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(9)]
        public string PhoneNumber { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(6)]
        public string Postcode { get; set; }
        [MaxLength(75)]
        public string EmailAddress { get; set; }
        public ICollection<Subjects> Subjects { get; set; }
    }
}