using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVC_MyMusicStore.Data;
using MVC_MyMusicStore.Models;
using System.Diagnostics;
using System.Net;

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

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {

            if (string.IsNullOrEmpty(sortOrder))
            {
                if (Request.Cookies.TryGetValue("SortOrder", out string storedSortOrder))
                {
                    sortOrder = storedSortOrder;
                }
                else
                {
                    sortOrder = "title_desc"; // Default sort order if not found in cookies
                }
            }
            Console.WriteLine("cookie" + sortOrder);


            ViewData["TitleSortParam"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParam"] = sortOrder == "Price_Low" ? "Price_High"  : "Price_Low";


            //include genres            
            var genres = _db.Genres.ToList();
            ViewData["Genres"] = genres;
            
            IQueryable<Album> albums =  _db.Albums.Include(a => a.Artist).Include(a => a.Genre);
            if (!string.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(a => a.Title.Contains(searchString)).OrderBy(a => a.Title);
            }

            //cases of sorting 
            switch (sortOrder)
            {
                case "title_desc":
                    albums = albums.OrderByDescending(a => a.Title);
                    break;
                case "Price_Low":
                    albums = albums.OrderBy(a => a.Price); 
                    break;
                case "Price_High":
                    albums = albums.OrderByDescending(a => a.Price); 
                    break;
                default:
                    albums = albums.OrderBy(a => a.Title);
                    break;
            }

            //store cookie
            
            Response.Cookies.Append("SortOrder", sortOrder, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(1) // Expires after 1 day
            });
            return View(albums.ToList());
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
