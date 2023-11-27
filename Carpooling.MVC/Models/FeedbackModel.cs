using System;
using Carpooling.Data.Models;

namespace Carpooling.MVC.Models
{
    public class FeedbackModel
    {
        public FeedbackModel()
        {

        }

        public FeedbackModel(Feedback feedback)
        {
            this.Id = feedback.Id;
            this.TripId = feedback.TripId;
            this.Rating = feedback.Rating;
            this.Description = feedback.Description;
            this.Role = feedback.Role.TravelRole.ToString();
            this.Departure = feedback.Trip.Departure;
            this.StartDestination = feedback.Trip.StartDestination;
            this.EndDestination = feedback.Trip.EndDestination;
        }
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public DateTime Departure { get; set; }
        public string StartDestination { get; set; }
        public string EndDestination { get; set; }
    }
}
