using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace Cultural_Center.Models
{
    public class StudentGroups
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(9)]
        public string DayOfTheWeek { get; set; }
        
        [MaxLength(5)]
        [Required]
        public string StartTime { get; set; }
        
        [MaxLength(5)]
        [Required]
        public string EndTime { get; set; }
        
        public int LessonsId { get; set; }
        public Lessons Lesson { get; set; }
        public ICollection<Enrollments> Enrollments { get; set; }

    }
}