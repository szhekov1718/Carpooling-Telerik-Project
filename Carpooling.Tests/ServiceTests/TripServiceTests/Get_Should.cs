using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Models;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.TripServiceTests
{
    [TestClass]
    public class Get_Should
    {
        [TestMethod]
        public async Task ReturnAllTrips_Should()
        {
            var options = Utils.GetOptions(nameof(ReturnAllTrips_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                var result = await sut.GetAllTripsAsync();

                Assert.AreEqual(actContext.Trips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", trips), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task ReturnAllAvailableTrips_Should()
        {
            var options = Utils.GetOptions(nameof(ReturnAllAvailableTrips_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                var result = await sut.GetAllAvailableTripsAsync();
                var availableTrips = await actContext.Trips.Where(t => t.Departure > DateTime.Now.Date && t.FreeSpots > 0).ToListAsync();

                Assert.AreEqual(availableTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", availableTrips.Select(t => new TripDTO(t))), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task ReturnAllAvailableTripsListEmpty_Should()
        {
            var options = Utils.GetOptions(nameof(ReturnAllAvailableTripsListEmpty_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.GetAllAvailableTripsAsync());
            }
        }

        [TestMethod]
        public async Task ReturnIfNoTrips_Should()
        {
            var options = Utils.GetOptions(nameof(ReturnIfNoTrips_Should));

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.GetAllTripsAsync());
            }
        }

        [TestMethod]
        public async Task ReturnASingleTrip_Should()
        {
            var options = Utils.GetOptions(nameof(ReturnASingleTrip_Should));
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
                var result = await sut.GetTripAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442146"));
                var trip = await actContext.Trips.FirstOrDefaultAsync(t => t.Id == Guid.Parse("943b692d-330e-405d-a019-c3d728442146"));

                Assert.AreEqual(trip.StartDestination, result.StartDestination);
                Assert.AreEqual(trip.EndDestination, result.EndDestination);
                Assert.AreEqual(trip.Departure, result.Departure);
                Assert.AreEqual(trip.FreeSpots, result.FreeSpots);
            }
        }

        [TestMethod]
        public async Task ReturnTripWithWrongParams_Should()
        {
            var options = Utils.GetOptions(nameof(ReturnTripWithWrongParams_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.GetTripAsync(Guid.Parse("943b692d-330e-405d-a022-c3d728442146"))); // using wrong Guid
            }
        }

        [TestMethod]
        public async Task AvailableTripsAsync_Should()
        {
            var options = Utils.GetOptions(nameof(AvailableTripsAsync_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                List<Trip> availableTrips = new List<Trip>()
                {
                    new Trip
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    StartDestination = "Plovdiv",
                    EndDestination = "Sofia",
                    Departure = new DateTime(2021, 12, 15),
                    FreeSpots = 3,
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                 }
                };

                var result = await sut.AvailableTripsAsync();
                Assert.AreEqual(availableTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", availableTrips), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task AvailableTripsAsync_ShouldThrowTravelDoesNotExistException()
        {
            var options = Utils.GetOptions(nameof(AvailableTripsAsync_ShouldThrowTravelDoesNotExistException));

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);


                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.AvailableTripsAsync());
            }
        }
    }
}
