using Carpooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpooling.Data.Configuration
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> userRole)
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.Property(ur => ur.Rating).HasDefaultValue(0);

            userRole.Property(ur => ur.RatingSum).HasDefaultValue(0);

            userRole.Property(ur => ur.FeedbacksCount).HasDefaultValue(0);
        }
    }
}
