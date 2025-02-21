using Microsoft.AspNetCore.Mvc;
using MvcUnitTesting_dotnet8.Models;
using System.Diagnostics;
using Tracker.WebAPIClient;

namespace MvcUnitTesting_dotnet8.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRepository<Book> repository;

        public HomeController(IRepository<Book> bookRepo, ILogger<HomeController> logger)
        {
            ActivityAPIClient.Track(StudentID: "S00243021", StudentName: "Dmytro severin", activityName: "Rad302 2025 Week 2 Lab 1", Task: " Running initial tests");

            repository = bookRepo;
            _logger = logger;
        }
        
        public IActionResult Index(string genre)
        {
            ViewData["Genre"] = !string.IsNullOrEmpty(genre) ? genre : "All";

            var books = repository.GetAll();
            return View(books);
        }

        public IActionResult Privacy()
        {
            ViewData["Message"] = "Your Privacy is our concern";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
