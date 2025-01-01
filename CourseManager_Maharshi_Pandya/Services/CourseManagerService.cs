using CourseManager_Maharshi_Pandya.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

namespace CourseManager_Maharshi_Pandya.Services
{
    public class CourseManagerService : ICourseManagerService
    {
        private readonly CourseManagerDbContext _context;
        private readonly IConfiguration _configuration;

        public CourseManagerService(CourseManagerDbContext context, IConfiguration configuration)
        {

            _context = context;
            _configuration = configuration;
        }
        public List<Course> GetAllCourses()
        {
            return _context.Courses.Include(c => c.Students).OrderByDescending(c => c.StartDate).ToList();
        }
        public Course GetCourseById(int courseId)
        {
            return _context.Courses.Include(c => c.Students).FirstOrDefault(c => c.CourseID == courseId);
        }
        public int AddCourse(Course NewCourse)
        {
            _context.Courses.Add(NewCourse);
            _context.SaveChanges();

            return NewCourse.CourseID;
        }
        public int UpdateCourse(Course courseToEdit)
        {
            _context.Courses.Update(courseToEdit);
            _context.SaveChanges();

            return courseToEdit.CourseID;
        }

        public void AddStudentToCourse(int courseId, Student newStudent)
        {
            var currentCourse = GetCourseById(courseId);

            if (currentCourse == null)
            {
                return;
            }
            currentCourse.Students.Add(newStudent);
            _context.SaveChanges();
        }
        public void SendEnrollmentEmails(int courseId)
        {
            Console.WriteLine("Sending enrollment emails to all guests of event with id: " + courseId);

        }

        public Student GetStudentById(int studentId)
        {
            return _context.Students.FirstOrDefault(s => s.StudentID == studentId);
        }
        public void UpdateStudent(Student studentToEdit)
        {
            _context.Students.Update(studentToEdit);
            _context.SaveChanges();
        }

        public void SendEnrollmentEmail(int courseId, string hostname, string scheme)
        {
            var currentCourse = GetCourseById(courseId);
            if (currentCourse == null) { return; }

            var students = currentCourse.Students.Where(g => g.Status == EnrollmentConfirmationStatus.ConfirmationMessageNotSent).ToList();

            try
            {
                foreach (var student in students)
                {
                    // getting these configuration from appsettings.json
                    var smtpHost = _configuration["SmtpSettings:Host"];
                    var smtpPort = _configuration["SmtpSettings:Port"];
                    var smtpFromPassword = _configuration["SmtpSettings:FromPassword"];
                    var smtpFromAddress = _configuration["SmtpSettings:FromAddress"];

                    if (_configuration["SmtpSettings:FromAddress"] == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"SMTP From Address: {smtpFromAddress}");
                        Console.ForegroundColor = ConsoleColor.White;
                        throw new Exception("SMTP From Address is null");
                    }


                    using var smtpClient = new SmtpClient(smtpHost); //Using keyowrd disposses after use
                    smtpClient.Port = string.IsNullOrEmpty(smtpPort) ? 587 : Convert.ToInt32(smtpPort);
                    smtpClient.Credentials = new NetworkCredential(smtpFromAddress, smtpFromPassword);
                    smtpClient.EnableSsl = true;

                    var message = new MailMessage()
                    {
                        From = new MailAddress(smtpFromAddress),
                        Subject = $"[Action Required] Confirm {currentCourse.CourseName} enrollment",
                        Body = @$"
<h1>Hello {student.StudentName}! </h1> 
<p> Your request to enroll in the course {currentCourse.CourseName} in room {currentCourse.RoomNumber} starting on {currentCourse.StartDate?.ToShortDateString()} with instructor {currentCourse.InstructorName}.</p>" +

@$"<p> We are pleased to have you in the course so if you could <a href='http://localhost:5076/courses/{currentCourse.CourseID}/enroll-confirmation/{student.StudentID}'>confirm your enrollment</a> as soon as possible that would be appereciated! </p> " +

@"<p> Sincerly,</p>" + "<p> The Course Manager App </p>",
                        IsBodyHtml = true // this property renders gmail messages
                    };
                    if (student.StudentEmail == null)
                    {
                        throw new Exception("Student email is null");
                    }
                    message.To.Add(new MailAddress(student.StudentEmail));

                    //Send the email
                    smtpClient.Send(message);

                    student.Status = EnrollmentConfirmationStatus.ConfirmationMessageSent;
                    _context.Update(student);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void ConfirmEmail(int courseId, int studentId, string response)
        {


            var currentCourse = GetCourseById(courseId);

            if (currentCourse == null) { return; }

            var student = currentCourse.Students.Find(g => g.StudentID == studentId);

            if (student == null) { return; }

            if (response.ToLower() == "yes")
            {
                student.Status = EnrollmentConfirmationStatus.EnrollmentConfirmed;
            }
            else
            {
                student.Status = EnrollmentConfirmationStatus.EnrollmentDeclined;
            }

            _context.Update(student);
            _context.SaveChanges();
        }


    }


}


