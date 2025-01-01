using CourseManager_Maharshi_Pandya.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseManager_Maharshi_Pandya.Controllers
{
    public class HomeController : AbstractBaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel()
            {
                UserTrackingMessage = GetUserTrackingMessage()
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            var viewModel = new HomeViewModel()
            {
                UserTrackingMessage = GetUserTrackingMessage()
            };
            return View("Privacy", viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
