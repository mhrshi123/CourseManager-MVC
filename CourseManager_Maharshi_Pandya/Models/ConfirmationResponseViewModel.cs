using CourseManager_Maharshi_Pandya.Entities;
using System.ComponentModel.DataAnnotations;

namespace CourseManager_Maharshi_Pandya.Models
{
    public class ConfirmationResponseViewModel : AbstractBaseViewModel
    {
        public Course? ActiveCourse { get; set; }

        public Student? Student { get; set; }

        [Required(ErrorMessage ="Please select yes or no!")]
        public string? Response { get; set; } = string.Empty;
    }
}
