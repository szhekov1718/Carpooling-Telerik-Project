using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Carpooling.Data.Contracts;

namespace Carpooling.Data.Models
{
    public class User : IDeletable
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(20)]
        public string Username { get; set; } // make unique?

        [Required]
        public string Password { get; set; } // contain capital, digit and symbol?

        [Required, MaxLength(20)]
        public string FirstName { get; set; }

        [Required, MaxLength(20)]
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public bool IsBlocked { get; set; }
        public string Image { get; set; }
        public ICollection<Trip> Trips { get; set; } = new HashSet<Trip>();
        public ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();

        [InverseProperty(nameof(TripCandidate.Passanger))]
        public ICollection<TripCandidate> TripCandidates { get; set; } = new HashSet<TripCandidate>();
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public bool IsDeleted { get; set; }
    }
}
