using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Cultural_Center.Models
{
    public class Students
    {
        [Key] 
        public int Id { get; set; }
        [MaxLength(15)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
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
        public ICollection<StudentGroups> StudentGroups { get; set; }
    }
}