using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.TripServiceTests
{
    [TestClass]
    public class Delete_Should
    {
        [TestMethod]
        public async Task DeleteTrip_Should()
        {
            var options = Utils.GetOptions(nameof(DeleteTrip_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var tripCandidates = Utils.SeedTripCandidates();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.TripCandidates.AddRangeAsync(tripCandidates);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                var result = await sut.DeleteAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442148"));

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task DeleteTripWithInvalidParams_Should()
        {
            var options = Utils.GetOptions(nameof(DeleteTripWithInvalidParams_Should));
            var trips = Utils.SeedTrips();

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.DeleteAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442146")));
            }
        }
    }
}
