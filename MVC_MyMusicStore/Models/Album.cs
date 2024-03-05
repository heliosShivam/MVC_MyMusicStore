using System.ComponentModel.DataAnnotations;

namespace MVC_MyMusicStore.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }

        [Required]
        public string? Title { get; set; }
        public decimal Price { get; set; }

        public Genre? Genre { get; set; }
        public Artist? Artist { get; set; }
    }
}
