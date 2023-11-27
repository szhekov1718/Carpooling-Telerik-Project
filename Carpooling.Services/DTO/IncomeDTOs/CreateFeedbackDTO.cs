using System;
using Carpooling.Data.Models;

namespace Carpooling.Services.DTO.IncomeDTOs
{
    public class CreateFeedbackDTO
    {
        public CreateFeedbackDTO()
        {

        }
        public CreateFeedbackDTO(Feedback feedback)
        {
            this.Rating = feedback.Rating.ToString();
            this.Description = feedback.Description;
            this.Username = feedback.User.Username;
            this.TripId = feedback.TripId;
        }
        public string Rating { get; set; }
        public string Description { get; set; }
        public Guid TripId { get; set; }
        public string Username { get; set; }
    }
}
