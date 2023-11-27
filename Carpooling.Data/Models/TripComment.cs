using System;
using System.ComponentModel.DataAnnotations;
using Carpooling.Data.Contracts;

namespace Carpooling.Data.Models
{
    public class TripComment : IDeletable
    {
        public Guid Id { get; set; }

        [Required]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Guid TripId { get; set; }
        public Trip Trip { get; set; }
        public bool IsDeleted { get; set; }
    }
}
