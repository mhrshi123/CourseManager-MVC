using CourseManager_Maharshi_Pandya.Entities;
using CourseManager_Maharshi_Pandya.Models;
using CourseManager_Maharshi_Pandya.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CourseManager_Maharshi_Pandya.Controllers
{
    public class CoursesController : AbstractBaseController
    {
        private readonly ICourseManagerService _courseManagerService;
        public CoursesController(ICourseManagerService courseManager)
        {
            _courseManagerService = courseManager;
        }

        [HttpGet("/courses")]

        public IActionResult AllCourses()
        {
            var viewModel = new CoursesViewModel()
            {
                UserTrackingMessage = GetUserTrackingMessage(),

                Courses = _courseManagerService.GetAllCourses()
            };

            return View("All", viewModel);

        }

        [HttpGet("/courses/add")]
        public IActionResult AddCourse()
        {
            var viewModel = new CourseViewModel()
            {
                UserTrackingMessage = GetUserTrackingMessage(),

                Course = new Course()
            };
            return View("Add", viewModel);
        }

        [HttpPost("courses/add")]
        public IActionResult AddCourse(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.UserTrackingMessage = GetUserTrackingMessage();
                return View("Add", viewModel);

            }
            else
            {
                viewModel.UserTrackingMessage = GetUserTrackingMessage();


                _courseManagerService.AddCourse(viewModel.Course);
                TempData["Message"] = $"The course \"{viewModel.Course.CourseName}\" added successfully!";
                TempData["className"] = "success";

                return RedirectToAction("ManageCourse", new { viewModel.Course.CourseID });
            }
        }


        [HttpGet("/courses/{courseId}/edit")]
        public IActionResult EditCourse(int courseId)
        {

            var activeCourse = _courseManagerService.GetCourseById(courseId);

            if (activeCourse == null)
            {

                return NotFound();
            }
            var viewModel = new CourseViewModel()
            {
                UserTrackingMessage = GetUserTrackingMessage(),

                Course = activeCourse
            };

            return View("Edit", viewModel);
        }


        [HttpPost("/courses/{courseId}/edit")]
        public IActionResult EditCourse(int courseId, CourseViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }
            else
            {
                _courseManagerService.UpdateCourse(viewModel.Course);
                TempData["Message"] = $"The course \"{viewModel.Course.CourseName}\" has been modified successfully!";
                TempData["className"] = "info";
                return RedirectToAction("ManageCourse", new { courseId });
            }

        }


        [HttpGet("/courses/{courseId}/manage")]
        public IActionResult ManageCourse(int courseId)
        {

            var activeCourse = _courseManagerService.GetCourseById(courseId);

            if (activeCourse == null)
            {
                return NotFound();
            }
            // Ensure Students list is initialized
            if (activeCourse.Students == null)
            {
                activeCourse.Students = new List<Student>();
            }
            var viewModel = new ManageCourseViewModel()
            {
                UserTrackingMessage = GetUserTrackingMessage(),

                Course = activeCourse,

                Student = new Student(),



                CountConfirmationMessageNotSent = activeCourse.Students.Count(s => s.Status == EnrollmentConfirmationStatus.ConfirmationMessageNotSent),
                CountConfirmationMessageSent = activeCourse.Students.Count(s => s.Status == EnrollmentConfirmationStatus.ConfirmationMessageSent),
                CountEnrollmentConfirmed = activeCourse.Students.Count(s => s.Status == EnrollmentConfirmationStatus.EnrollmentConfirmed),
                CountEnrollmentDeclined = activeCourse.Students.Count(s => s.Status == EnrollmentConfirmationStatus.EnrollmentDeclined)

            };

            return View("Manage", viewModel);
        }

        [HttpPost("/courses/{courseId}/manage/add-student")]
        public IActionResult AddStudent(int courseId, ManageCourseViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                var activeCourse = _courseManagerService.GetCourseById(courseId);

                if (activeCourse == null)
                {
                    return NotFound();
                }

                viewModel.Course = activeCourse;
                viewModel.Student = new Student();
                viewModel.CountConfirmationMessageNotSent = activeCourse.Students.Count(s => s.Status == EnrollmentConfirmationStatus.ConfirmationMessageNotSent);
                viewModel.CountConfirmationMessageSent = activeCourse.Students.Count(s => s.Status == EnrollmentConfirmationStatus.ConfirmationMessageSent);
                viewModel.CountEnrollmentConfirmed = activeCourse.Students.Count(s => s.Status == EnrollmentConfirmationStatus.EnrollmentConfirmed);
                viewModel.CountEnrollmentDeclined = activeCourse.Students.Count(s => s.Status == EnrollmentConfirmationStatus.EnrollmentDeclined);
                return View("Manage", viewModel);
            }


            _courseManagerService.AddStudentToCourse(courseId, viewModel.Student);

            return RedirectToAction("ManageCourse", new { courseId });

        }

        [HttpPost("/Courses/Manage/{courseId}/Enroll")]

        public IActionResult EnrollStudent(int courseId)
        {
            var hostname = Request.Host.ToString();
            var scheme = Request.Scheme;

            _courseManagerService.SendEnrollmentEmail(courseId, hostname, scheme);
            return RedirectToAction("ManageCourse", new { courseId });

        }

        [HttpGet("/courses/{courseId}/enroll-confirmation/{studentId}")]

        public IActionResult EnrollCourseConfirmation(int courseId, int studentId)
        {
            var course = _courseManagerService.GetCourseById(courseId);
            var student = _courseManagerService.GetStudentById(studentId);
            if (course == null)
            {
                return NotFound();
            }
            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new ConfirmationResponseViewModel
            {
                ActiveCourse = course,

                Student = student,

                Response = string.Empty,

                UserTrackingMessage = GetUserTrackingMessage(),


            };

            return View("Confirmation", viewModel);

        }

        [HttpPost("/courses/{courseId}/enroll-confirmation/{studentId}")]

        public IActionResult EnrollCourseConfirmation(int courseId, int studentId, ConfirmationResponseViewModel viewModel)
        {

            var student = _courseManagerService.GetStudentById(studentId);
            if (student == null)
            {
                return NotFound();
            }

            viewModel.Student = student;
            if (!ModelState.IsValid)
            {

                viewModel.UserTrackingMessage = GetUserTrackingMessage();
                viewModel.ActiveCourse = _courseManagerService.GetCourseById(courseId);
                viewModel.Student = _courseManagerService.GetStudentById(studentId);



                return View("Confirmation", viewModel);
            }

            _courseManagerService.ConfirmEmail(courseId, studentId, viewModel.Response);

            if (viewModel.Response.ToLower() == "yes")
            {
                TempData["ThankYouMessage"] = "We are thrilled to hear that you will be joining us!";
                viewModel.Student.Status = EnrollmentConfirmationStatus.EnrollmentConfirmed;
                _courseManagerService.UpdateStudent(viewModel.Student);

            }
            else
            {
                viewModel.Student.Status = EnrollmentConfirmationStatus.EnrollmentDeclined;
                TempData["ThankYouMessage"] = "We feel very sorry to hear that you won't be joing us :(";
                _courseManagerService.UpdateStudent(viewModel.Student);
            }

            return RedirectToAction("ThankYou");
        }

        [HttpGet("/courses/thank-you")]
        public IActionResult ThankYou()
        {
            var viewModel = new ConfirmationResponseViewModel
            {
                UserTrackingMessage = GetUserTrackingMessage()
            };

            return View("ThankYou", viewModel);
        }

    }
}
