using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations;

namespace MVC_MyMusicStore.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string? Name { get; set; }

    }
}
