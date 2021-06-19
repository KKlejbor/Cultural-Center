using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cultural_Center.Models
{
    public class Subjects
    {
        [Key]
        public int Id { get; set;}
        [MaxLength(30)]
        public string Name { get; set;}
        public int InstructorsId { get; set; }
        public Instructors Instructor { get; set; }
        public ICollection<Lessons> Lessons { get; set; }
    }
}