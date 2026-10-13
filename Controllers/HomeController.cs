using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using homework.Models;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;
=======

>>>>>>> 3ed1585bec5e2fdd0ff6b542e38bd61b94aedac6
namespace homework.Controllers;

public class HomeController : Controller
{
<<<<<<< HEAD

    [Authorize (Roles = "Admin")]
=======
>>>>>>> 3ed1585bec5e2fdd0ff6b542e38bd61b94aedac6
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
