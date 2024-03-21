using Microsoft.AspNetCore.Mvc;
using MVC_MyMusicStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MVC_MyMusicStore.Models;
using MVC_MyMusicStore.Models.CartModels;
using Microsoft.AspNetCore.Authorization;

namespace MVC_MyMusicStore.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public UserOrderController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            IQueryable<OrderDetail> orders = _db.OrderDetail.Include(o => o.Order).Include(o => o.Album)
                                               .Where(o => o.Order.Username == user.UserName);
                                              
            return View(await orders.OrderByDescending(o => o.Order.OrderDate).ToListAsync());
            
        }
    }
}
