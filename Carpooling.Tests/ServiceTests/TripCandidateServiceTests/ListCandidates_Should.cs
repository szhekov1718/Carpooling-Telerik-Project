using System;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Carpooling.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.TripCandidateServiceTests
{
    [TestClass]
    public class ListCandidates_Should
    {
        [TestMethod]
        public async Task ListApprovedCandidates_Should()
        {
            var options = Utils.GetOptions(nameof(ListApprovedCandidates_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var candidates = Utils.SeedTripCandidates();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.AddRangeAsync(candidates);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripCandidateService(actContext, sutHelp);

                var result = await sut.ListTripApprovedCandidatesAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442148"));
                Assert.AreEqual("Stanimir", result[1].FirstName);
                Assert.AreEqual("Ivanov", result[1].LastName);
                Assert.AreEqual("miro44@abv.bg", result[1].Email);
                Assert.AreEqual("Mirko", result[1].Username);
                Assert.AreEqual("0864646464", result[1].PhoneNumber);
            }
        }

        [TestMethod]
        public async Task ListRejectedCandidates_Should()
        {
            var options = Utils.GetOptions(nameof(ListRejectedCandidates_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var candidates = Utils.SeedTripCandidates();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.AddRangeAsync(candidates);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripCandidateService(actContext, sutHelp);

                var result = await sut.ListTripRejectedCandidatesAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442146"));
                Assert.AreEqual("Stanimir", result[0].FirstName);
                Assert.AreEqual("Ivanov", result[0].LastName);
                Assert.AreEqual("miro44@abv.bg", result[0].Email);
                Assert.AreEqual("Mirko", result[0].Username);
                Assert.AreEqual("0864646464", result[0].PhoneNumber);
            }
        }

        [TestMethod]
        public async Task ListAllCandidates_Should()
        {
            var options = Utils.GetOptions(nameof(ListAllCandidates_Should));
            var candidates = Utils.SeedTripCandidates();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripCandidateService(actContext, sutHelp);
                var result = await sut.GetAllTripCandidatesAsync();

                Assert.AreEqual(actContext.TripCandidates.Count(), result.Count());
                Assert.AreEqual(string.Join(",", actContext.TripCandidates.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task ListAllCandidatesEmpty_Should()
        {
            var options = Utils.GetOptions(nameof(ListAllCandidatesEmpty_Should));

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripCandidateService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.GetAllTripCandidatesAsync());
            }
        }

        [TestMethod]
        public async Task ListApprovedCandidatesEmpty_Should()
        {
            var options = Utils.GetOptions(nameof(ListApprovedCandidatesEmpty_Should));

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripCandidateService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<NoApprovedCandidates>(() => sut.ListTripApprovedCandidatesAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442148")));
            }
        }

        [TestMethod]
        public async Task ListRejectedCandidatesEmpty_Should()
        {
            var options = Utils.GetOptions(nameof(ListRejectedCandidatesEmpty_Should));

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripCandidateService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.ListTripRejectedCandidatesAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442146")));
            }
        }
    }
}
