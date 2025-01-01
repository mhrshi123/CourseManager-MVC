using System.ComponentModel.DataAnnotations;

namespace CourseManager_Maharshi_Pandya.Entities
{
    public enum EnrollmentConfirmationStatus
    {
        ConfirmationMessageNotSent,
        ConfirmationMessageSent,
        EnrollmentConfirmed,
        EnrollmentDeclined
    }
    public class Student
    {
        public int StudentID { get; set; }

        [Required(ErrorMessage = "What's your name?")]
        public string? StudentName { get; set; }



        [Required(ErrorMessage = "What's your email?")]
        [EmailAddress(ErrorMessage = "Please enter a valid email addresss")]
        public string? StudentEmail { get; set; }


        [Required(ErrorMessage = "What is your entrollment status?")]
        public EnrollmentConfirmationStatus Status { get; set; } = EnrollmentConfirmationStatus.ConfirmationMessageNotSent;

        public int CourseID { get; set; }

        public Course? Course { get; set; }

    }
}
