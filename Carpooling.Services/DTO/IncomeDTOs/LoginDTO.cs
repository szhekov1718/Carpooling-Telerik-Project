namespace Carpooling.Services.DTO.IncomeDTOs
{
    public class LoginDTO
    {
        public LoginDTO()
        {

        }
        public LoginDTO(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
