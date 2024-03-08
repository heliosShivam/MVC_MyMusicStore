using MVC_MyMusicStore.Models.CartModels;

namespace MVC_MyMusicStore.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
         
        public decimal CartTotal { get; set; }
    }
}
