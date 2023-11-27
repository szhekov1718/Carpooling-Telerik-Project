using Carpooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpooling.Data.Configuration
{
    public class TripCandidateConfig : IEntityTypeConfiguration<TripCandidate>
    {
        public void Configure(EntityTypeBuilder<TripCandidate> tripCandidate)
        {
            tripCandidate.HasKey(t => new { t.TripId, t.UserId });

            tripCandidate.Property(t => t.IsApproved)
                   .HasDefaultValue(false);

            tripCandidate.HasOne(tc => tc.Passanger)
                .WithMany(tc => tc.TripCandidates)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
