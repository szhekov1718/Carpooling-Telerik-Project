using Carpooling.Data.Models;

namespace Carpooling.Services.DTO.IncomeDTOs
{
    public class UserDTO
    {
        public UserDTO()
        {

        }
        public UserDTO(User user)
        {
            this.Username = user.Username;
            this.Password = user.Password;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.Blocked = user.IsBlocked;
            this.IsAdmin = user.IsAdmin;
            this.ImageUrl = user.Image;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Blocked { get; set; }
        public bool IsAdmin { get; set; }
        public string ImageUrl { get; set; }
    }
}
