using System.ComponentModel.DataAnnotations;

namespace MVC_MyMusicStore.Models.CartModels
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public bool isDeleted { get; set; } = false;

        public string UserId { get; set;}
    }
}
