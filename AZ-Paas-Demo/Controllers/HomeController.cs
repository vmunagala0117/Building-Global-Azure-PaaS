using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AZ_Paas_Demo.Models;
using Microsoft.Extensions.Configuration;

namespace AZ_Paas_Demo.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration _Configuration;
        public HomeController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }
        public IActionResult Index()
        {
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
