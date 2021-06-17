using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Cultural_Center.Models
{
    public class StudentGroups
    {
        [Key]
        public int Id { get; set; }
        public short DayOfTheWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int LessonsId { get; set; }
        public Lessons Lesson { get; set; }

        public ICollection<Students> Students { get; set; }
    }
}