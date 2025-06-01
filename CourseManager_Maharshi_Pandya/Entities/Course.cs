using System.ComponentModel.DataAnnotations;

namespace CourseManager_Maharshi_Pandya.Entities
{
    public class Course
    {
        public int CourseID { get; set; }

        [Required(ErrorMessage = "Please enter the course name!")]
        public string? CourseName { get; set; }

        [Required(ErrorMessage = "Please enter the instructor name!")]
        public string? InstructorName { get; set; }

        [Required(ErrorMessage = "Please enter the start date!")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please enter the room number!")]
        [RegularExpression("^\\d[A-Z]\\d{2}$", ErrorMessage = "Roomnumber must be a single digit, a single capital letter, and two digits")]
        public string? RoomNumber { get; set; }

        public List<Student>? Students { get; set; }

    }
}
