using System;
using Carpooling.Data.Models;

namespace Carpooling.Services.DTO.IncomeDTOs
{
    public class FeedbackDTO
    {
        public FeedbackDTO()
        {

        }
        public FeedbackDTO(Feedback feedback)
        {
            this.Rating = feedback.Rating.ToString();
            this.Description = feedback.Description;
            this.StartDestination = feedback.Trip.StartDestination;
            this.EndDestination = feedback.Trip.EndDestination;
            this.Username = feedback.User.Username;
            this.RoleId = feedback.RoleId;
        }
        public string Rating { get; set; }
        public string Description { get; set; }
        public string StartDestination { get; set; }
        public string EndDestination { get; set; }
        public string Username { get; set; }
        public Guid RoleId { get; set; }
    }
}
