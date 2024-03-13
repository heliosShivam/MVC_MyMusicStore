using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_MyMusicStore.Data;
using MVC_MyMusicStore.Models;
using System.Diagnostics;

namespace MVC_MyMusicStore.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger , AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var genres = _db.Genres.ToList();
            ViewData["Genres"] = genres;
            
            var items = _db.Albums.Include(a => a.Genre).ToList();
            
            
            return View(items);
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

       /* public IActionResult GetGenre()
        {
           
            return View("_GetGenre", genres);

        }*/
    }
}
