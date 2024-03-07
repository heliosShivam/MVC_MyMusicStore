namespace MVC_MyMusicStore.Models.CartModels
{
    public class CartDetail
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int AlbumId { get; set; }

        public int Quantity { get; set; }
        public Album Album { get; set; }
        public Cart Cart { get; set; }
    }
}
