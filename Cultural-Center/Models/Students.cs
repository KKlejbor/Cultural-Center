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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string EmailAddress { get; set; }
        public ICollection<StudentGroups> StudentGroups { get; set; }
    }
}