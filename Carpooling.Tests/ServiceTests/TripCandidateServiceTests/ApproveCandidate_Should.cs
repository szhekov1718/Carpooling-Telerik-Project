using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Carpooling.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.TripCandidateServiceTests

{
    [TestClass]
    public class ApproveCandidate_Should
    {
        [TestMethod]
        public async Task ApprovesCandidate_Should()
        {
            var options = Utils.GetOptions(nameof(ApprovesCandidate_Should));
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

                var result = await sut.ApproveCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442659"), Guid.Parse("943b692d-330e-405d-a019-c3d728442148"), Guid.Parse("943b692d-330e-405d-a019-c3d728442145"));

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task ApprovesCandidateWrongParams_Should()
        {
            var options = Utils.GetOptions(nameof(ApprovesCandidateWrongParams_Should));
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

                await Assert.ThrowsExceptionAsync<TravelPassengerDoesNotExistException>(() => sut.ApproveCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728552666"), Guid.Parse("943b692d-330e-405d-a019-c3d728442148"), Guid.Parse("943b692d-330e-405d-a019-c3d728442145")));
            }
        }

        [TestMethod]
        public async Task ApprovesCandidateWrongDriverId_Should()
        {
            var options = Utils.GetOptions(nameof(ApprovesCandidateWrongDriverId_Should));
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

                await Assert.ThrowsExceptionAsync<AuthorisationException>(() => sut.ApproveCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442666"), Guid.Parse("943b692d-330e-405d-a019-c3d728442148"), Guid.Parse("943b692d-330e-405d-a019-c3d728440000")));
            }
        }

        [TestMethod]
        public async Task ApprovesCandidateWrongTripId_Should()
        {
            var options = Utils.GetOptions(nameof(ApprovesCandidateWrongTripId_Should));
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

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.ApproveCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728552666"), Guid.Parse("943b692d-330e-405d-a019-c3d728442000"), Guid.Parse("943b692d-330e-405d-a019-c3d728442145")));
            }
        }
    }
}
