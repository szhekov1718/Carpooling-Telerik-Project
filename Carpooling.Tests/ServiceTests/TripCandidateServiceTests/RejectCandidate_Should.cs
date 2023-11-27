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
    public class RejectCandidate_Should
    {
        [TestMethod]
        public async Task RejectingCandidate_Should()
        {
            var options = Utils.GetOptions(nameof(RejectingCandidate_Should));
            var candidates = Utils.SeedTripCandidates();
            var trips = Utils.SeedTrips();

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
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.SaveChangesAsync();
            }

            using(var randomContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(randomContext);
                var sut = new TripCandidateService(randomContext, sutHelp);



                var result = await sut.RejectCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442153"), Guid.Parse("943b692d-330e-405d-a019-c3d728442148"), Guid.Parse("943b692d-330e-405d-a019-c3d728442145"));

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task RejectingCandidateWrongDriverId_Should()
        {
            var options = Utils.GetOptions(nameof(RejectingCandidateWrongDriverId_Should));
            var candidates = Utils.SeedTripCandidates();
            var trips = Utils.SeedTrips();

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
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.SaveChangesAsync();
            }

            using(var randomContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(randomContext);
                var sut = new TripCandidateService(randomContext, sutHelp);

                await Assert.ThrowsExceptionAsync<AuthorisationException>(() => sut.RejectCandidateAsync(Guid.Parse("943b622d-330e-405d-a019-c3d728442233"), Guid.Parse("943b692d-330e-405d-a019-c3d728442148"), Guid.Parse("943b692d-330e-405d-a019-c3d728442222")));
            }
        }

        [TestMethod]
        public async Task RejectingCandidateWrongCandidateId_Should()
        {
            var options = Utils.GetOptions(nameof(RejectingCandidateWrongCandidateId_Should));
            var candidates = Utils.SeedTripCandidates();
            var trips = Utils.SeedTrips();

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
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.SaveChangesAsync();
            }

            using(var randomContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(randomContext);
                var sut = new TripCandidateService(randomContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelPassengerDoesNotExistException>(() => sut.RejectCandidateAsync(Guid.Parse("943b622d-330e-405d-a019-c3d728440000"), Guid.Parse("943b692d-330e-405d-a019-c3d728442148"), Guid.Parse("943b692d-330e-405d-a019-c3d728442145")));
            }
        }

        [TestMethod]
        public async Task RejectingCandidateWrongDate_Should()
        {
            var options = Utils.GetOptions(nameof(RejectingCandidateWrongDate_Should));
            var candidates = Utils.SeedTripCandidates();
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.SaveChangesAsync();
            }

            using(var randomContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(randomContext);
                var sut = new TripCandidateService(randomContext, sutHelp);

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.RejectCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442151"), Guid.Parse("943b692d-220e-405d-a019-c3d728442146"), Guid.Parse("943b692d-330e-405d-a019-c3d728445689")));
            }
        }

        [TestMethod]
        public async Task RejectingCandidateNullTrip_Should()
        {
            var options = Utils.GetOptions(nameof(RejectingCandidateNullTrip_Should));
            var candidates = Utils.SeedTripCandidates();
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.SaveChangesAsync();
            }

            using(var randomContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(randomContext);
                var sut = new TripCandidateService(randomContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.RejectCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442151"), Guid.Parse("943b632d-330e-405d-a019-c3d728442146"), Guid.Parse("943b692d-330e-405d-a019-c3d728445689")));
            }
        }

        [TestMethod]
        public async Task RejectsCandidateWrongParams_Should()
        {
            var options = Utils.GetOptions(nameof(RejectsCandidateWrongParams_Should));
            var candidates = Utils.SeedTripCandidates();
            var trips = Utils.SeedTrips();


            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.TripCandidates.AddRangeAsync(candidates);

                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripCandidateService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.ApproveCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442266"), Guid.Parse("943b692d-220e-405d-a019-c3d728442146"), Guid.Parse("943b692d-330e-405d-a019-c3d728445689")));
            }
        }
    }
}
