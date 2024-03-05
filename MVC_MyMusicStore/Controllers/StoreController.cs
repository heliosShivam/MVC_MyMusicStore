using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_MusicStore.Data;
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
        public ActionResult Browse(string genre)
        {
            var genreModel = _db.Genres.Include("Albums")
        .Single(g => g.Name == genre);
            return View(genreModel);
        }

        public IActionResult Details(int id)
        {
            var album = new Album { Title = "Album " + id };
            return View(album);
        }
    }
}
