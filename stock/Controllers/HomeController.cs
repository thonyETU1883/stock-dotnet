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
        return View();
    }

    public IActionResult Versformulairesortie()
    {
        Magasin magasin = new Magasin();
        ViewBag.listemagasin = magasin.getallmagasin(null);
        return View("Sortieformulaire");
    }

    public IActionResult insertionsortie(String date,String idarticle,double quantite,double prixunitaire,String idmagasin)
    {   
        Mouvement mouvement = new Mouvement(DateTime.Parse(date),quantite,idarticle,idmagasin,prixunitaire);
        mouvement.insertionsortie(null);
        return View("Index");
    }

    public IActionResult Versvalidationsortie()
    {
        Mouvement mouvement = new Mouvement();
        List<Mouvement> listemouvement = mouvement.getallmouvementnovalider(null); 
        ViewBag.listemouvement = listemouvement;
        return View("validationsortie");
    }

     public IActionResult validation(String idmouvement)
    {
        try{
            Mouvement mouvement = new Mouvement();
            mouvement.getsortiebyid(idmouvement,null);
            mouvement.validationsortie(null);
        }catch(Exceptionpersonnalise e){
            Console.WriteLine(e.Message);
        }
        return View("Index");
    }

    public IActionResult versetatdestockformulaire()
    {   
        Magasin magasin = new Magasin();
        ViewBag.listemagasin = magasin.getallmagasin(null);
        return View("Etatdestockformulaire");
    }

    public IActionResult etatdestock(String dateinitial,String datefinal,String idarticle,String idmagasin)
    {   
        Magasin magasin = new Magasin();
        magasin.getmagasinbyid(idmagasin,null);

        List<Etatdestock> listeetatdestockinitial = magasin.getetatdestockbyidarticle(idarticle,dateinitial,null);
        List<Etatdestock> listeetatdestockfinal = magasin.getetatdestockbyidarticle(idarticle,datefinal,null);
        double montant = magasin.sommemontant(idarticle,datefinal,null);
        ViewBag.listeetatdestockinitial = listeetatdestockinitial;
        ViewBag.listeetatdestockfinal = listeetatdestockfinal;
        ViewBag.montant = montant;
        return View("Etatdestocktable");
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
