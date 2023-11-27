using System.Collections.Generic;
using Carpooling.Services;
using Carpooling.Services.DTO.IncomeDTOs;

namespace Carpooling.MVC.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<UserDTO> Users { get; set; }

        public IEnumerable<TripDTO> Trips { get; set; }

        public IEnumerable<UserRatingDTO> Ratings { get; set; }
        public IEnumerable<FeedbackDTO> Feedbacks { get; set; }
        public int AverageRating { get; set; }
    }
}
