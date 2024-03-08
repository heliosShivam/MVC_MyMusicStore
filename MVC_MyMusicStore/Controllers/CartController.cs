using Microsoft.AspNetCore.Mvc;
using MVC_MyMusicStore.Models;
using MVC_MyMusicStore.Models.ViewModels;
using System.Text.Encodings.Web;
using MVC_MyMusicStore.Data;

namespace MVC_MyMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly AppDbContext _storeDB;

        public ShoppingCartController(AppDbContext storeDB)
        {
            _storeDB = storeDB;
        }

        // GET: /ShoppingCart/
        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(HttpContext.RequestServices); 

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        public IActionResult AddToCart(int id)
        {
            
            var addedAlbum = _storeDB.Albums
                .SingleOrDefault(album => album.AlbumId == id);

            if (addedAlbum == null)
            {
                return NotFound();
            }
           
            var cart = ShoppingCart.GetCart(HttpContext.RequestServices);
            cart.AddToCart(addedAlbum);

            return RedirectToAction("Index");
        }


    }
}
