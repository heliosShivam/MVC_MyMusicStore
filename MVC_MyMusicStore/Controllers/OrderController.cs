using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_MyMusicStore.Data;

namespace MVC_MyMusicStore.Controllers
{
    [Authorize(Policy = "Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var orders = _db.OrderDetail.Include(o => o.Order).ToList();
            return View(orders);
        }

        [HttpPost]
        public IActionResult Delete(int orderId)
        {
            var order = _db.OrderDetail.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                _db.OrderDetail.Remove(order);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AcceptOrder(int orderId)
        {
            var order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.Status = "Accepted";
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult RejectOrder(int orderId)
        {
            var order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.Status = "Rejected";
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
    
}
