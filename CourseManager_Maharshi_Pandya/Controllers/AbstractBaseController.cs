using Microsoft.AspNetCore.Mvc;

namespace CourseManager_Maharshi_Pandya.Controllers
{
    public class AbstractBaseController : Controller
    {

        private const string FirstVisitCookieName = "FirstVisitTimestamp";

        public string GetUserTrackingMessage()
        {
            var firstVisitCookie = Request.Cookies[FirstVisitCookieName];
            if (firstVisitCookie == null)
            {

                var firstVisitTimestamp = DateTime.Now.ToString("MM/dd/yyyy");
                Response.Cookies.Append(FirstVisitCookieName, firstVisitTimestamp, new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1)
                });

                return $"Welcome to our page for the first time on {firstVisitTimestamp}!";
            }
            else
            {

                return $"Welcome back! You first used this app on {firstVisitCookie}.";
            }
        }

    }
}




