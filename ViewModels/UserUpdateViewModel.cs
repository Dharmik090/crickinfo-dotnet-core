using crickinfo_mvc_ef_core.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.ViewModels
{
    public class UserUpdateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty; // Non-nullable with default initialization

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
