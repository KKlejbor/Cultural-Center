using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Cultural_Center.Models
{
    public class StudentGroups
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(9)]
        public string DayOfTheWeek { get; set; }
        
        [Required]
        [Column(TypeName = "time")]
        [DisplayFormat(DataFormatString="{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9](?::00)?$", ErrorMessage = "Invalid time format. The format should be HH:MM. Where HH refers to hours in 24 hour format and MM refers to minutes")]
        
        public TimeSpan StartTime { get; set; }
        
        [Required]
        [Column(TypeName = "time")]
        [DisplayFormat(DataFormatString="{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9](?::00)?$", ErrorMessage = "Invalid time format. The format should be HH:MM. Where HH refers to hours in 24 hour format and MM refers to minutes")]
        public TimeSpan EndTime { get; set; }
        
        public int LessonsId { get; set; }
        public Lessons Lesson { get; set; }
        public ICollection<Enrollments> Enrollments { get; set; }

    }
}