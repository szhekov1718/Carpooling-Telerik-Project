using System;
using System.ComponentModel.DataAnnotations;
using Carpooling.Data.Contracts;

namespace Carpooling.Data.Models
{
    public class TripCandidate : IDeletable
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsApproved { get; set; }
        public Guid UserId { get; set; }
        public User Passanger { get; set; }
        public Guid DriverId { get; set; }
        public User Driver { get; set; }
        public Guid TripId { get; set; }
        public Trip Trip { get; set; }
        public bool IsDeleted { get; set; }
    }
}
