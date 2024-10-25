using crickinfo_mvc_ef_core.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models
{
	public class User
	{
		public User(){}
		public User(int Id)
		{
			this.Id = Id;
		}
		public User(string Name,string Email,string Password,bool IsAdmin)
		{
			this.Name = Name;
			this.Email = Email;
			this.Password = Password;
			this.IsAdmin = IsAdmin;
		}
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; } = string.Empty; // Non-nullable with default initialization

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
        [UniqueEmail]
        public string Email { get; set; } = string.Empty; 

		[Required(ErrorMessage = "Password is required")]
		[StringLength(100, MinimumLength = 6)]
		public string Password { get; set; } = string.Empty; 

		public bool IsAdmin { get; set; }
	}
}
