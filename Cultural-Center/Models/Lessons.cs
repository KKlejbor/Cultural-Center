using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cultural_Center.Models
{
    public class Lessons
    {
        [Key]
        public int Id { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        
        public int NumberOfParticipants { get; set; }
        public string ClassroomNumber { get; set; }
        public int SubjectsId { get; set; }
        public Subjects Subject { get; set; }
        public ICollection<StudentGroups> StudentGroups { get; set; }
    }
}