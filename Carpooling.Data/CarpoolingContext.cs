using System.Reflection;
using Carpooling.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Carpooling.Data
{
    public class CarpoolingContext : DbContext
    {
        public CarpoolingContext(DbContextOptions<CarpoolingContext> options) : base(options)
        {

        }

        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TripCandidate> TripCandidates { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripComment> TripComments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Seed();
        }
    }
}
