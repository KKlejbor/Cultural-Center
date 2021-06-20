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
        
        public ICollection<StudentGroupsStudents> StudentGroupsStudents { get; set; }
    }
}