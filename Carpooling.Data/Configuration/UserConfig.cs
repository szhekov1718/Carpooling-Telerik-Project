using Carpooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpooling.Data.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user.Property(u => u.IsAdmin)
                   .HasDefaultValue(false);

            user.Property(u => u.IsDeleted)
                   .HasDefaultValue(false);

            user.Property(u => u.IsBlocked)
                   .HasDefaultValue(false);
        }
    }
}
