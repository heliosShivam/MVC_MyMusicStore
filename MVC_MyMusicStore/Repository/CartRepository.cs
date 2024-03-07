using Microsoft.AspNetCore.Identity;
using MVC_MusicStore.Data;
using MVC_MyMusicStore.Models.CartModels;

namespace MVC_MyMusicStore.Repository
{
    public class CartRepository
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _context;

        public CartRepository(AppDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor context)
        {
            _db = db;
            _userManager = userManager;
            _context = context;
        }

        public void AddItem()
        {
            string userId = "";
            var cart = GetCart(userId);
        }

        public Cart GetCart(string userId)
        {
            var cart = _db.carts.FirstOrDefault( c => c.UserId == userId);
            return cart;
        }
    }
}
