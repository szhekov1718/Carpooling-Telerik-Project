using System.Collections.Generic;
using Carpooling.Services.DTO.IncomeDTOs;

namespace Carpooling.MVC.Models
{
    public class UserView
    {
        public ICollection<UserDTO> userDTOs { get; set; }
        public string SearchAttribute { get; set; }
        public string Value { get; set; }
    }
}
