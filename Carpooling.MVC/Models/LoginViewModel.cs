using System.ComponentModel.DataAnnotations;

namespace Carpooling.MVC.Models
{
    public class LoginViewModel
    {
        [Required, MaxLength(20, ErrorMessage = "Email length must be {1}!")]
        public string Username { get; set; }

        [Required, MinLength(8, ErrorMessage = "Password length must be at least {1} characters!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password is invalid!")]
        public string Password { get; set; }
    }
}
