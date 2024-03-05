using Microsoft.AspNetCore.Mvc;
using MVC_MusicStore.Data;
using MVC_MyMusicStore.Models;
using MVC_MyMusicStore.Models.CartModels;
using System.Text.Json;
namespace MVC_MyMusicStore.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        public CartController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart(int Pid)
        {

            

            return RedirectToAction("Index");
        }
    }
}
