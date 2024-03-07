namespace MVC_MyMusicStore.Models.CartModels
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public int AlbumId { get; set; }
        public int Quntity { get; set; }

        public decimal UnitPrice { get; set; }

        public Album Album { get; set; }
        public Order Order { get; set; }    
        
    }
}
