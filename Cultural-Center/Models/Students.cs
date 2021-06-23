using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace Cultural_Center.Models
{
    public class Students
    {
        [Key] 
        public int Id { get; set; }
        
        [MaxLength(15)]
        [Required]
        public string FirstName { get; set; }
        
        [MaxLength(30)]
        [Required]
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime BirthDate { get; set; }
        
        [MaxLength(9)]
        [RegularExpression(@"\d{9}", ErrorMessage = "Invalid phone number. It should consist of 9 digits from 0-9")]
        [Required]
        public string PhoneNumber { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string Address { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string City { get; set; }
        
        [MaxLength(6)]
        [RegularExpression(@"\d{2}-\d{3}", ErrorMessage = "Invalid post code number. It should follow the format: XX-XXX where X is a digit from 0-9")]
        [Required]
        public string Postcode { get; set; }
        
        [MaxLength(75)]
        [RegularExpression(@"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?", ErrorMessage = "Invalid email address")]
        [Required]
        public string EmailAddress { get; set; }
        
        public ICollection<Enrollments> Enrollments { get; set; }
    }
}