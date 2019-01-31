using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AZ_Paas_Demo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AZ_Paas_Demo.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration _Configuration;
        ILogger<HomeController> _logger;
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _Configuration = configuration;
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation(string.Format("This is my region: {0}", _Configuration.GetValue<string>("Region")));
            ViewData["Region"] = _Configuration.GetValue<string>("Region");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            _logger.LogWarning("Contact page under construction!!");
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Error:",Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
