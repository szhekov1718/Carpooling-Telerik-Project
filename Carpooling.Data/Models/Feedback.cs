using System;
using System.ComponentModel.DataAnnotations;
using Carpooling.Data.Contracts;

namespace Carpooling.Data.Models
{
    public class Feedback : IDeletable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Range(0, 5, ErrorMessage = "Please enter a value between 0 and 5.")]
        public int Rating { get; set; }
        public string Description { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Guid TripId { get; set; }
        public Trip Trip { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public bool IsDeleted { get; set; }
    }
}
