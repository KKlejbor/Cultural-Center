using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cultural_Center.Models
{
    public class Enrollments
    {
        [Key]
        public int StudentGroupsId { get; set; }
        [Key]
        public int StudentsId { get; set; }

        public StudentGroups StudentGroup { get; set; }

        public Students Student { get; set; }
    }
}
