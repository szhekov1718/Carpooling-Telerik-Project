using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Carpooling.Data.Contracts;
using Carpooling.Data.Enums;

namespace Carpooling.Data.Models
{
    public class Role : IDeletable
    {
        [Key]
        public Guid Id { get; set; }
        public TravelRole TravelRole { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
