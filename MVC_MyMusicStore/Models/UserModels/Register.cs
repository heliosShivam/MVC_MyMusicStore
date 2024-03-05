using System.ComponentModel.DataAnnotations;

namespace MVC_MyMusicStore.Models.UserModels
{
    public class Register
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password" , ErrorMessage ="Doesn't match with password")]
        public string ConfirmPassword {  get; set; }    
    }
}
