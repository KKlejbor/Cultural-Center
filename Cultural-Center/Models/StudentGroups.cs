using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Cultural_Center.Models
{
    public class StudentGroups
    {
        [Key]
        public int Id { get; set; }
        public byte DayOfTheWeek { get; set; }
        [MaxLength(5)]
        public string StartTime { get; set; }
        [MaxLength(5)]
        public string EndTime { get; set; }
        public int LessonsId { get; set; }
        public Lessons Lesson { get; set; }
        public ICollection<Students> Students { get; set; }
    }
}