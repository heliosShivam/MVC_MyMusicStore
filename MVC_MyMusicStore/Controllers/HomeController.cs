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

        public async Task<IActionResult> Index(string searchString)
        {
            var genres = _db.Genres.ToList();
            ViewData["Genres"] = genres;
            
            //var items = _db.Albums.Include(a => a.Genre).ToList();
            IQueryable<Album> items =  _db.Albums.Include(a => a.Artist).Include(a => a.Genre);
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(a => a.Title.Contains(searchString)).OrderBy(a => a.Title);
            }
            return View(items.ToList());
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
