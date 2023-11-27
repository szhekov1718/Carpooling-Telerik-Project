using System.ComponentModel.DataAnnotations;

namespace Carpooling.MVC.Models
{
    public class UserViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} value must be between {2} and {1} symbols!")]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Username must be between {2} and {1}!")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Username must be between {2} and {1}!")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Phone number is invalid!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number is invalid!")]
        public string PhoneNumber { get; set; }

        public string ImageUrl { get; set; }
    }
}
