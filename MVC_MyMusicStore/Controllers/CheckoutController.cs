using Microsoft.AspNetCore.Mvc;
using MVC_MyMusicStore.Data;
using MVC_MyMusicStore.Models;
using MVC_MyMusicStore.Models.CartModels;

namespace MVC_MyMusicStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _db;
        const string PromoCode = "FREE";

        public CheckoutController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Order or)
        {
            var order = new Order();
            TryUpdateModelAsync(order);
           /* var xyz = values["PromoCode"];*/
            
            string p = Request.Form["PromoCode"];
            Console.Write(p);
            if (!string.Equals(p, PromoCode, StringComparison.OrdinalIgnoreCase))
            {
                return View(order);
            }
            else
            {
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;

                _db.Orders.Add(order);
                _db.SaveChanges();

                var cart = ShoppingCart.GetCart(HttpContext.RequestServices);
                cart.CreateOrder(order);

                return RedirectToAction("Complete", new { id = order.OrderId });


            }
            
        }

        public IActionResult Complete(int id)
        {
            
            bool isValid = _db.Orders.Any(o => o.OrderId == id && o.Username == User.Identity.Name);

            if(isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
