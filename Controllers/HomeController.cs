using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace mvc1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return "Hello, ASP.NET Core MVC!";
            return View();
        }

    }
}
