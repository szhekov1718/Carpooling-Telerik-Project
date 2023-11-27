using Carpooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpooling.Data.Configuration
{
    public class FeedbackConfig : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> feedback)
        {
            feedback.HasOne(f => f.User)
                   .WithMany(f => f.Feedbacks)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
