using System.Collections.Generic;
using Carpooling.Data.Models;

namespace Carpooling.Services.DTO.IncomeDTOs
{
    public class UserRatingDTO
    {
        public UserRatingDTO(UserRole userRole)
        {
            Username = userRole.User.Username;
            Rating = userRole.Rating;
        }
        public UserRatingDTO(double rating, string username, List<string> comments)
        {
            Username = username;
            Rating = rating;
            Comments = new List<string>(comments);
        }

        public string Username { get; set; }
        public double Rating { get; set; }
        public List<string> Comments { get; set; }

    }
}
