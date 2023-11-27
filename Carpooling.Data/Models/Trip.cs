using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Carpooling.Data.Contracts;
using Carpooling.Data.Enums;

namespace Carpooling.Data.Models
{
    public class Trip : IDeletable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string StartDestination { get; set; }

        [Required]
        public string EndDestination { get; set; }

        [Required]
        public DateTime Departure { get; set; }
        public int FreeSpots { get; set; }
        public TravelStatus TravelStatus { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();
        public ICollection<TripCandidate> TripCandidates { get; set; } = new HashSet<TripCandidate>();
        public Guid DriverId { get; set; }
        public User Driver { get; set; }
    }
}
