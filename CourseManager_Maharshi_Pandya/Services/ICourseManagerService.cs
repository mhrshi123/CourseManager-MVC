using CourseManager_Maharshi_Pandya.Entities;

namespace CourseManager_Maharshi_Pandya.Services
{
    public interface ICourseManagerService
    {
        public List<Course> GetAllCourses();
        public int AddCourse(Course NewCourse);
        public int UpdateCourse(Course courseToEdit);
        public Course GetCourseById(int courseId);

        public void AddStudentToCourse(int courseId, Student newStudent);

        public void SendEnrollmentEmails(int courseId);

        public Student GetStudentById(int studentId);

        public void UpdateStudent(Student studentToEdit);

        public void SendEnrollmentEmail(int courseId, string hostname, string scheme);

        public void ConfirmEmail(int courseId, int studentId, string response);
    }
}
