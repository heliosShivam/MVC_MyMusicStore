using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_MyMusicStore.Data;
using MVC_MyMusicStore.Models;
using System.Web;

namespace MVC_MyMusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _db;

        public StoreController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            
            var genres = _db.Genres.ToList();

            return View(genres);

        }
        public IActionResult Browse(string genre)
        {
            var fullUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
            HttpContext.Session.SetString("LastVisitedPage", fullUrl);

            var genreModel = _db.Genres.Include("Albums")
            .FirstOrDefault(g => g.Name == genre);

            if (genreModel == null)
            {
                return RedirectToAction("BrowseNotFound");
            }
            return View(genreModel);
            
        }

        public IActionResult BrowseNotFound()
        {
            return View();
        }

        public IActionResult Details(int id)
        {

            /*var album = new Album { Title = "Album " + id };
            return View(album);*/
            HttpContext.Session.SetString("LastVisitedPage", HttpContext.Request.Path);
            var album = _db.Albums
            .Include(a => a.Genre)//Included genre to get genre names
            .Include(a => a.Artist)// Include the artist to get artist name
            .FirstOrDefault(a => a.AlbumId == id);

            if (album == null)
            {
                return NotFound(); // when album is not found
            }

            return View(album);
        }

        
        // GET: /Store/GenreMenu

        /*public IActionResult GenreMenu()
        {
            var genres = _db.Genres.ToList();

            return PartialView(genres);


        }*/
    }
}
