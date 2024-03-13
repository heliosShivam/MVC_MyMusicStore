
using MVC_MyMusicStore.Models.CartModels;
using MVC_MyMusicStore.Models;
using MVC_MyMusicStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVC_MyMusicStore.Models
{
    public partial class ShoppingCart
    {
        private readonly AppDbContext _db;
        private readonly string _shoppingCartId;
        private string ShoppingCartId { get; set; }
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public const string CartSessionKey = "CartId";

        public ShoppingCart(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _shoppingCartId = GetCartId();
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            var contextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            var Storedb = services.GetRequiredService<AppDbContext>();
            var cart = new ShoppingCart(Storedb, contextAccessor);
            cart.ShoppingCartId = cart.GetCartId();
            return cart; 
        }

        public void AddToCart(Album album)
        {
            var cartItem = _db.Carts.SingleOrDefault(
                 c => c.CartId == ShoppingCartId && c.AlbumId == album.AlbumId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = _shoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                _db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            _db.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = _db.Carts.SingleOrDefault(cart => cart.CartId == _shoppingCartId && cart.RecordId == id);
            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _db.Carts.Remove(cartItem);
                }

                _db.SaveChanges();
            }

            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = _db.Carts.Where(cart => cart.CartId == _shoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _db.Carts.Remove(cartItem);
            }

            _db.SaveChanges();
        }

        public IQueryable<Cart> GetCartItems()
        {
            return _db.Carts.Include(x => x.Album).Where(cart => cart.CartId == _shoppingCartId).OrderBy(x => x.Album.Title);
        }

        public int GetCount()
        {
            int? count = (from cartItems in _db.Carts
                          where cartItems.CartId == _shoppingCartId
                          select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in _db.Carts
                              where cartItems.CartId == _shoppingCartId
                              select (int?)cartItems.Count * cartItems.Album.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };

                orderTotal += (item.Count * item.Album.Price);
                _db.OrderDetail.Add(orderDetail);
            }

            order.Total = orderTotal;
            _db.SaveChanges();
            EmptyCart();

            return order.OrderId;
        }

        public string GetCartId()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            return context.Session.GetString(CartSessionKey);
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = _db.Carts.Where(c => c.CartId == _shoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }

            _db.SaveChanges();
        }
    }
}
