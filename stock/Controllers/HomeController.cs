using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using stock.Models;

namespace stock.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        Article article = new Article("vm","vary mena",1);
        Mouvement mouvement = article.getlastmouvement(null,"magasin1");
        Console.WriteLine(mouvement.getIdmouvement());
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
