using System.ComponentModel.DataAnnotations;

namespace Carpooling.MVC.Models
{
    public class ContactUsViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Value for {0} must be between {1} and {2} characters.")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Value for {0} must be between {1} and {2} characters.")]
        public string Topic { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public string Description { get; set; }
    }
}
