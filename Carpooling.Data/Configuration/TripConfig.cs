using Carpooling.Data.Enums;
using Carpooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpooling.Data.Configuration
{
    public class TripConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> trip)
        {
            trip.HasOne(t => t.Driver)
                   .WithMany(t => t.Trips)
                   .OnDelete(DeleteBehavior.NoAction);

            trip.Property(t => t.TravelStatus)
                   .HasDefaultValue(TravelStatus.NotStarted);
        }
    }
}
