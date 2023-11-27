using System;

namespace Carpooling.MVC.Models
{
    public class TripCandidatesViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsApproved { get; set; }
        public double Rating { get; set; }
    }
}
