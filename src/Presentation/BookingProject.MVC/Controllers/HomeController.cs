using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookingProject.MVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
