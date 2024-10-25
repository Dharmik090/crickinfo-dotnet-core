using crickinfo_mvc_ef_core.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models.CreateModels
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
