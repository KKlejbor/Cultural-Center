using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Cultural_Center.Models
{
    public class Subjects
    {
        [Key]
        public int Id { get; set;}
        
        [MaxLength(30)]
        [Required]
        public string Name { get; set;}
        
        public int InstructorsId { get; set; }
        public Instructors Instructor { get; set; }
        public ICollection<Lessons> Lessons { get; set; }
    }
}