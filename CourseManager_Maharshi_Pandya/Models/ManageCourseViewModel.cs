using CourseManager_Maharshi_Pandya.Entities;

namespace CourseManager_Maharshi_Pandya.Models
{
    public class ManageCourseViewModel : AbstractBaseViewModel
    {
        public Course? Course { get; set; }

        public Student? Student { get; set; }

        public int CountConfirmationMessageNotSent { get; set; }

        public int CountConfirmationMessageSent { get; set; }

        public int CountEnrollmentConfirmed { get; set; }

        public int CountEnrollmentDeclined { get; set; }
    }
}
