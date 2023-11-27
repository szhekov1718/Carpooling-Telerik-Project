using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Models;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Carpooling.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.TripCandidateServiceTests
{
    [TestClass]
    public class PassengerDeclineCandidacy_Should
    {
        [TestMethod]
        public async Task DeclineTripCandidacy_Should()
        {
            var options = Utils.GetOptions(nameof(DeclineTripCandidacy_Should));
            var candidates = Utils.SeedTripCandidates();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            using(var arrContext = new CarpoolingContext(options))
            {
                var tripCandidate = new TripCandidate
                {
                    Id = Guid.Parse("943b622d-330e-405d-a019-c3d728442233"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442233"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                };
                arrContext.Add(tripCandidate);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.SaveChangesAsync();
            }

            using(var randomContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(randomContext);
                var sut = new TripCandidateService(randomContext, sutHelp);

                var result = await sut.PassengerDeclineTripCandidacyAsync("tisho", Guid.Parse("943b622d-330e-405d-a019-c3d728442233"));

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task DeclineTripCandidacyWrongTripCandidate_Should()
        {
            var options = Utils.GetOptions(nameof(DeclineTripCandidacyWrongTripCandidate_Should));
            var candidates = Utils.SeedTripCandidates();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            using(var arrContext = new CarpoolingContext(options))
            {
                var tripCandidate = new TripCandidate
                {
                    Id = Guid.Parse("943b622d-330e-405d-a019-c3d728442233"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442233"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                };
                arrContext.Add(tripCandidate);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.SaveChangesAsync();
            }

            using(var randomContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(randomContext);
                var sut = new TripCandidateService(randomContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.PassengerDeclineTripCandidacyAsync("tisho", Guid.Parse("943b622d-330e-405d-a019-c3d728440000")));
            }
        }


        [TestMethod]
        public async Task DeclineTripCandidacyWrongUsername_Should()
        {
            var options = Utils.GetOptions(nameof(DeclineTripCandidacyWrongUsername_Should));
            var candidates = Utils.SeedTripCandidates();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            using(var arrContext = new CarpoolingContext(options))
            {
                var tripCandidate = new TripCandidate
                {
                    Id = Guid.Parse("943b622d-330e-405d-a019-c3d728442233"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442233"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                };
                arrContext.Add(tripCandidate);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.SaveChangesAsync();
            }

            using(var randomContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(randomContext);
                var sut = new TripCandidateService(randomContext, sutHelp);

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.PassengerDeclineTripCandidacyAsync("preslava", Guid.Parse("943b622d-330e-405d-a019-c3d728442233")));
            }
        }
    }
}
