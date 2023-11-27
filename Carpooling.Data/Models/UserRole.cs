using System;

namespace Carpooling.Data.Models
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public double Rating { get; set; }
        public int FeedbacksCount { get; set; }
        public int RatingSum { get; set; }
    }
}
