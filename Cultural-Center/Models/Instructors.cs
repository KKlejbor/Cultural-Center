using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Cultural_Center.Models
{
    public class Instructors
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(15)]
        [Required]
        public string FirstName { get; set; }
        
        [MaxLength(30)]
        [Required]
        public string LastName { get; set; }
        
        [MaxLength(9)]
        [Required]
        public string PhoneNumber { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string Address { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string City { get; set; }
        
        [MaxLength(6)]
        [Required]
        public string Postcode { get; set; }
        
        [MaxLength(75)]
        [Required]
        public string EmailAddress { get; set; }
        
        public ICollection<Subjects> Subjects { get; set; }
    }
}