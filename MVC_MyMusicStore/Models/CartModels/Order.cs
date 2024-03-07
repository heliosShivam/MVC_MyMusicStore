namespace MVC_MyMusicStore.Models.CartModels
{
    public class Order
    {
        public int Id { get; set; }

        public string userId {  get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int OrderStatusId { get; set; }

        public bool isDeleted { get; set; }     
        public OrderStatus OrderStatuscs { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}
