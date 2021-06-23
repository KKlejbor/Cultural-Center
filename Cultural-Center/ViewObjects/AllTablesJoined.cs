using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cultural_Center.Models;

namespace Cultural_Center.ViewObjects
{
    public class AllTablesJoined
    {
        /*
         * Students table
         */
        public int IdStudents { get; set; }
        public string StudentsFirstName { get; set; }
        public string StudentsLastName { get; set; }
        public DateTime StudentsBirthDate { get; set; }
        public string StudentsPhoneNumber { get; set; }
        public string StudentsAddress { get; set; }
        public string StudentsCity { get; set; }
        public string StudentsPostcode { get; set; }
        public string StudentsEmailAddress { get; set; }
        public int StudentGroupsId { get; set; }
        
        /*
         * Enrollments table
         */
        public int StudentsId { get; set; }
        public int IdStudentGroups { get; set; }
        
        /*
         * StudentGroups table
         */
        public string DayOfTheWeek { get; set; }
        
        [DisplayFormat(DataFormatString="{0:hh\\:mm}")]
        public TimeSpan StartTime { get; set; }
        
        [DisplayFormat(DataFormatString="{0:hh\\:mm}")]
        public TimeSpan EndTime { get; set; }
        
        public int LessonsId { get; set; }
        
        /*
         * Lessons table
         */
        public int IdLessons { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        public int NumberOfParticipants { get; set; }
        public string ClassroomNumber { get; set; }
        public int SubjectsId { get; set; }
        
        /*
         * Subjects table
         */
        public int IdSubjects { get; set;}
        public string Name { get; set;}
        public int InstructorsId { get; set; }
        
        /*
         * Instructors table
         */
        public int IdInstructors { get; set; }
        public string InstructorsFirstName { get; set; }
        public string InstructorsLastName { get; set; }
        public string InstructorsPhoneNumber { get; set; }
        public string InstructorsAddress { get; set; }
        public string InstructorsCity { get; set; }
        public string InstructorsPostcode { get; set; }
        public string InstructorsEmailAddress { get; set; }
    }
}