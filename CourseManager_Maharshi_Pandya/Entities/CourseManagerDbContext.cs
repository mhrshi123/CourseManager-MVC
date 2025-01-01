using Microsoft.EntityFrameworkCore;

namespace CourseManager_Maharshi_Pandya.Entities
{
    public class CourseManagerDbContext : DbContext
    {

        public CourseManagerDbContext(DbContextOptions<CourseManagerDbContext> optionn) : base(optionn)

        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseID = 1,
                    CourseName = "Math",
                    InstructorName = "Mr. Smith",
                    StartDate = new DateTime(2021, 9, 1),
                    RoomNumber = "1A13"
                },
                new Course
                {
                    CourseID = 2,
                    CourseName = "Science",
                    InstructorName = "Mr. Johnson",
                    StartDate = new DateTime(2021, 9, 1),
                    RoomNumber = "2B24"
                },
                new Course
                {
                    CourseID = 3,
                    CourseName = "History",
                    InstructorName = "Mr. Davis",
                    StartDate = new DateTime(2021, 9, 1),
                    RoomNumber = "3C45"
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentID = 1,
                    StudentName = "John Doe",
                    StudentEmail = "Djohn@gmail.com",
                    Status = EnrollmentConfirmationStatus.ConfirmationMessageSent,
                    CourseID = 1
                },

                new Student
                {
                    StudentID = 2,
                    StudentName = "Maharshi Pandya",
                    StudentEmail = "mhrshi3112@gmail.com",
                    Status = EnrollmentConfirmationStatus.ConfirmationMessageNotSent,
                    CourseID = 1
                },

                new Student
                {
                    StudentID = 3,
                    StudentName = "Jake Harlow",
                    StudentEmail = "JH12@gmail.com",
                    Status = EnrollmentConfirmationStatus.EnrollmentConfirmed,
                    CourseID = 2
                }
                );
        }
    }
}
