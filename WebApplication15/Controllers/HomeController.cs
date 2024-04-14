using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication15.Models;

namespace WebApplication15.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        //Lab 5 - Project or ProjectTask Search
        // General search action
        [HttpGet]
        public IActionResult GeneralSearch(string searchType, string searchString)
        {
            if (searchType == "Projects")
            {
                // Redirect to Projects search
                return RedirectToAction("Search", "Projects", new { area = "ProjectManagement", searchString });
            }
            else if (searchType == "Tasks")
            {
                // Redirect to Tasks search - Assuming default projectId
                // You may need to modify this based on your application's logic
                var url = Url.Action("Search", "Task", new { area = "ProjectManagement" }) + $"?searchString={searchString}";

                // Use Redirect method to navigate to the constructed URL
                return Redirect(url);
            }

            return RedirectToAction("Index", "Home");
        }




        //Lab 5 - NotFound() Action added
        public IActionResult NotFound(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }

            return View("Error");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
