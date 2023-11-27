using System.ComponentModel.DataAnnotations;

namespace Carpooling.MVC.Models
{
    public class CreateViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, MinLength(8, ErrorMessage = "Password length must be at least {1} characters!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password is invalid!")]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string ImageUrl { get; set; }
    }
}
