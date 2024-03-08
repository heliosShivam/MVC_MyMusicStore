using Microsoft.AspNetCore.Mvc;
using MVC_MyMusicStore.Models;
using MVC_MyMusicStore.Models.ViewModels;
using System.Text.Encodings.Web;
using MVC_MyMusicStore.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
                CartTotal = cart.GetTotal(),
                
            };
            /*ViewBag.AlbumId = new SelectList(_storeDB.Albums, "AlbumId", "Title");*/
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

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(HttpContext.RequestServices);

            // Get the name of the album to display confirmation
            var cartItem = _storeDB.Carts
                .Where(item => item.RecordId == id)
                .Select(item => item.Album.Title)
                .SingleOrDefault();

            if (cartItem == null)
            {
                return NotFound();
            }

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                DeleteMessage = HtmlEncoder.Default.Encode(cartItem) +
                          " has been removed from your shopping cart.",
                CartTotalPrice = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

    }
}
