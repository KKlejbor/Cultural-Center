using System;
using System.ComponentModel.DataAnnotations;

namespace Cultural_Center.ViewObjects
{
    public class LessonsOffers
    {
        public string SubjectName { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }
        public int NumberOfParticipants { get; set; }
        public string ClassroomNumber { get; set; }
        //public int SubjectsId { get; set; }
        public string InstructorsFirstName { get; set; }
        public string InstructorsLastName { get; set; }
    }
}