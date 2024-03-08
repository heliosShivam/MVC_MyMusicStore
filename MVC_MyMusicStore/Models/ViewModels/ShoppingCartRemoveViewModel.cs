namespace MVC_MyMusicStore.Models.ViewModels
{
    public class ShoppingCartRemoveViewModel
    {
        public int CartTotalPrice { get; set; }
        public int ItemCount { get; set; }
        public int CartCount { get; set; }
        public int DeleteId { get; set; }
        public string DeleteMessage { get; set; }
    }
}
