using System;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Carpooling.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.TripCandidateServiceTests
{
    [TestClass]
    public class GetTripCandidate_Should
    {
        [TestMethod]
        public async Task GetTripCandidateMethod_Should()
        {
            var options = Utils.GetOptions(nameof(GetTripCandidateMethod_Should));
            var trips = Utils.SeedTrips();
            var candidates = Utils.SeedTripCandidates();

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
                var result = await sut.GetTripCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442149"));

                var candidacy = await actContext.TripCandidates.Where(tc => tc.Id == Guid.Parse("943b692d-330e-405d-a019-c3d728442149")).FirstOrDefaultAsync();

                Assert.AreEqual(result.Id, candidacy.Id);
                Assert.AreEqual(result.Trip, candidacy.Trip);
                Assert.AreEqual(result.DriverId, candidacy.DriverId);
            }
        }

        [TestMethod]
        public async Task GetTripCandidateWrongTripCandidate_Should()
        {
            var options = Utils.GetOptions(nameof(GetTripCandidateWrongTripCandidate_Should));
            var trips = Utils.SeedTrips();
            var candidates = Utils.SeedTripCandidates();

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

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.GetTripCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728440000")));
            }
        }

        [TestMethod]
        public async Task GetAllTripCandidates_Should()
        {
            var options = Utils.GetOptions(nameof(GetAllTripCandidates_Should));
            var trips = Utils.SeedTrips();
            var candidates = Utils.SeedTripCandidates();

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
                var result = await sut.GetAllTripCandidatesAsync();

                var candidacies = await actContext.TripCandidates.ToListAsync();

                Assert.AreEqual(result.Count(), candidacies.Count());
                Assert.AreEqual(string.Join(",", result), string.Join(",", candidacies));
            }
        }


        [TestMethod]
        public async Task GetAllTripCandidatesForTripThroweWHenNull_Should()
        {
            var options = Utils.GetOptions(nameof(GetAllTripCandidatesForTripThroweWHenNull_Should));
            var trips = Utils.SeedTrips();
            var candidates = Utils.SeedTripCandidates();

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

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.GetAllTripCandidatesForTripAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442146")));
            }
        }

        [TestMethod]
        public async Task GetTripCandidateTripThroweWHenNull_Should()
        {
            var options = Utils.GetOptions(nameof(GetTripCandidateTripThroweWHenNull_Should));
            var trips = Utils.SeedTrips();
            var candidates = Utils.SeedTripCandidates();

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

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.GetTripCandidateAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442151"), Guid.Parse("943b692d-330e-405d-a019-c3d728442146")));
            }
        }
    }
}
