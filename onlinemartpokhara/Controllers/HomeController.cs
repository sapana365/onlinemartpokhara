using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlinemartpokhara.Data;
using onlinemartpokhara.Models;
using OpenXmlPowerTools;
using System.Diagnostics;


namespace onlinemartpokhara.Controllers
{
   
    public class HomeController : Controller
    {
       
       

        public IActionResult Index()
        {
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