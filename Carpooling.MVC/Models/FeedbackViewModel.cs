using System;
using System.ComponentModel.DataAnnotations;
using Carpooling.Data.Models;

namespace Carpooling.MVC.Models
{
    public class FeedbackViewModel
    {
        public FeedbackViewModel()
        {

        }
        public FeedbackViewModel(Feedback feedback)
        {
            FeedbackId = feedback.Id;
            Rating = feedback.Rating;
            Comment = feedback.Description;
        }
        public Guid FeedbackId { get; set; }
        public Guid TripId { get; set; }
        [Required]
        public int Rating { get; set; }
        [MaxLength(200, ErrorMessage = "Comment length must be {1}!")]
        public string Comment { get; set; }
    }
}
