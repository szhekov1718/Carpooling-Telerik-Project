using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Models;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Carpooling.Tests.ServiceTests.TripServiceTests
{
    [TestClass]
    public class Update_Should
    {
        [TestMethod]
        public async Task UpdateTrip_Should()
        {
            var options = Utils.GetOptions(nameof(UpdateTrip_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var tripCandidates = Utils.SeedTripCandidates();

            var newTrip = new Mock<Trip>().Object;
            newTrip.StartDestination = "Burgas";
            newTrip.EndDestination = "Plovdiv";
            newTrip.Departure = DateTime.Now.Date.AddDays(2);
            newTrip.FreeSpots = 1;

            var newTripDto = new Mock<TripDTO>(newTrip).Object;

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.Users.AddRangeAsync(users);
                await arrangeContext.TripCandidates.AddRangeAsync(tripCandidates);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                var result = await sut.UpdateAsync(newTripDto, Guid.Parse("943b692d-330e-405d-a019-c3d728442146"));

                Assert.AreEqual(newTripDto.StartDestination, result.StartDestination);
                Assert.AreEqual(newTripDto.EndDestination, result.EndDestination);
                Assert.AreEqual(newTripDto.Departure, result.Departure);
                Assert.AreEqual(newTripDto.FreeSpots, result.FreeSpots);
            }
        }

        [TestMethod]
        public async Task ThrowWhenDTOIsNull_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenDTOIsNull_Should));

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.UpdateAsync(null, Guid.Parse("943b692d-330e-405d-a019-c3d728442146")));
            }
        }

        [TestMethod]
        public async Task ThrowWhenIdIsWrong_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenIdIsWrong_Should));
            var trips = Utils.SeedTrips();

            var newTrip = new Mock<Trip>().Object;
            newTrip.StartDestination = "Burgas";
            newTrip.EndDestination = "Plovdiv";
            newTrip.Departure = new DateTime(2021, 1, 1);
            newTrip.FreeSpots = 1;

            var newTripDto = new Mock<TripDTO>(newTrip).Object;

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<InvalidDepartureTimeException>(() => sut.UpdateAsync(newTripDto, Guid.Parse("943b692d-330e-405d-a019-c3d728442140")));
            }
        }

        [TestMethod]
        public async Task ThrowWhenFreeSpotsAreLessThanOne_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenFreeSpotsAreLessThanOne_Should));
            var trips = Utils.SeedTrips();

            var newTrip = new Mock<Trip>().Object;
            newTrip.StartDestination = "Burgas";
            newTrip.EndDestination = "Plovdiv";
            newTrip.Departure = new DateTime(2021, 1, 1);
            newTrip.FreeSpots = -1;

            var newTripDto = new Mock<TripDTO>(newTrip).Object;

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<InvalidDepartureTimeException>(() => sut.UpdateAsync(newTripDto, Guid.Parse("943b692d-330e-405d-a019-c3d728442140")));
            }
        }
    }
}
