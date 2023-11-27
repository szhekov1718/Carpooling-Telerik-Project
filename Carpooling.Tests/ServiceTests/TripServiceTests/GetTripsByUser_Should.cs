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
using Moq;

namespace Carpooling.Tests.ServiceTests.TripServiceTests
{
    [TestClass]
    public class GetTripsByUser_Should
    {
        [TestMethod]
        public async Task GetAllTripsByDriver_Should()
        {
            var options = Utils.GetOptions(nameof(GetAllTripsByDriver_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var tripCandidates = Utils.SeedTripCandidates();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                var result = await sut.GetTripsByUserRoleAsync("Stenlyto", "Driver");

                var stenlytoTrips = await actContext.Users.Where(u => u.Username == "Stenlyto")
                    .SelectMany(t => t.Trips)
                    .Select(t => new TripDTO(t))
                    .ToListAsync();

                Assert.AreEqual(stenlytoTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", stenlytoTrips.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task GetAllTripsByPassenger_Should()
        {
            var options = Utils.GetOptions(nameof(GetAllTripsByPassenger_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var tripCandidates = Utils.SeedTripCandidates();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.TripCandidates.AddRangeAsync(tripCandidates);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                var result = await sut.GetTripsByUserRoleAsync("Mirko", "passenger");

                var mirkoTrips = new List<TripDTO>
                {
                    new TripDTO
                    {
                        StartDestination = "Plovdiv",
                        EndDestination = "Sofia",
                        Departure = new DateTime(2021, 12, 15),
                        FreeSpots = 3
                    }
                };

                Assert.AreEqual(mirkoTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", mirkoTrips.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task GetAllTripsByPassengerEmptyList_Should()
        {
            var options = Utils.GetOptions(nameof(GetAllTripsByPassengerEmptyList_Should));
            var users = Utils.SeedUsers();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.GetTripsByUserRoleAsync("Mirko", "passenger"));
            }
        }

        [TestMethod]
        public async Task GetAllTripsByDriverEmptyList_Should()
        {
            var options = Utils.GetOptions(nameof(GetAllTripsByDriverEmptyList_Should));
            var users = Utils.SeedUsers();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.GetTripsByUserRoleAsync("Stenlyto", "driver"));
            }
        }

        [TestMethod]
        public async Task GetAllTripsByAllTripsEmptyList_Should()
        {
            var options = Utils.GetOptions(nameof(GetAllTripsByAllTripsEmptyList_Should));
            var users = Utils.SeedUsers();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.GetTripsByUserRoleAsync("Stenlyto", "all"));
            }
        }

        [TestMethod]
        public async Task GetAllTripsInvalidRole_Should()
        {
            var options = Utils.GetOptions(nameof(GetAllTripsInvalidRole_Should));
            var users = Utils.SeedUsers();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.GetTripsByUserRoleAsync("Stenlyto", "ledena kralica"));
            }
        }

        [TestMethod]
        public async Task GetAllTrips_Should()
        {
            var options = Utils.GetOptions(nameof(GetAllTrips_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var tripCandidates = Utils.SeedTripCandidates();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.TripCandidates.AddRangeAsync(tripCandidates);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                var result = await sut.GetTripsByUserRoleAsync("Stenlyto", "all");

                var allTrips = new List<TripDTO>
                {
                    new TripDTO
                    {
                        StartDestination = "Sofia",
                        EndDestination = "Plovdiv",
                        Departure = new DateTime(2021, 11, 10),
                        FreeSpots = 2
                    },
                    new TripDTO
                    {
                        StartDestination = "Plovdiv",
                        EndDestination = "Sofia",
                        Departure = new DateTime(2021, 12, 15),
                        FreeSpots = 3
                    }
                };

                Assert.AreEqual(allTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", allTrips.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task ThrowIfUserIsNull_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowIfUserIsNull_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Trips.AddRangeAsync(trips);

                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.GetTripsByUserRoleAsync(null, "driver"));
            }
        }

        [TestMethod]
        public async Task GetUserFutureTrips_Should()
        {
            var options = Utils.GetOptions(nameof(GetUserFutureTrips_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            var newTrip = new Mock<Trip>().Object;
            newTrip.StartDestination = "Varna";
            newTrip.EndDestination = "Burgas";
            newTrip.Departure = DateTime.Now.Date.AddDays(2);
            newTrip.FreeSpots = 3;

            var newTripDto = new Mock<TripDTO>(newTrip).Object;

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Trips.AddRangeAsync(trips);

                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                var getUser = await sutHelp.GetUserByUserNameAsync("Stenlyto");
                getUser.Trips.Add(newTrip);

                var addTripToUser = await actContext.Users.FirstAsync(u => u.Username == "Stenlyto");
                addTripToUser.Trips.Add(newTrip);

                await actContext.SaveChangesAsync();

                var result = await sut.GetUserFutureTripsAsync("Stenlyto");

                Assert.AreEqual("Varna", result[0].StartDestination);
                Assert.AreEqual("Burgas", result[0].EndDestination);
                Assert.AreEqual(DateTime.Now.Date.AddDays(2), result[0].Departure.Date);
                Assert.AreEqual(3, result[0].FreeSpots);
            }
        }

        [TestMethod]
        public async Task GetUserPastTrips_Should()
        {
            var options = Utils.GetOptions(nameof(GetUserPastTrips_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            var newTrip = new Mock<Trip>().Object;
            newTrip.StartDestination = "Varna";
            newTrip.EndDestination = "Burgas";
            newTrip.Departure = DateTime.Now.Date.AddDays(-2);
            newTrip.FreeSpots = 3;

            var newTripDto = new Mock<TripDTO>(newTrip).Object;

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Trips.AddRangeAsync(trips);

                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);


                var getUser = await sutHelp.GetUserByUserNameAsync("gogi");
                getUser.Trips.Add(newTrip);

                var addTripToUser = await actContext.Users.FirstAsync(u => u.Username == "gogi");
                addTripToUser.Trips.Add(newTrip);

                await actContext.SaveChangesAsync();

                var result = await sut.GetUserPastTripsAsync("gogi");

                Assert.AreEqual("Varna", result[0].StartDestination);
                Assert.AreEqual("Burgas", result[0].EndDestination);
                Assert.AreEqual(DateTime.Now.Date.AddDays(-2), result[0].Departure.Date);
                Assert.AreEqual(3, result[0].FreeSpots);
            }
        }

        [TestMethod]
        public async Task GetUserFutureTripsWrongParams_Should()
        {
            var options = Utils.GetOptions(nameof(GetUserFutureTripsWrongParams_Should));
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

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.GetUserFutureTripsAsync("gogi"));
            }
        }

        [TestMethod]
        public async Task GetUserPastTripsWrongParams_Should()
        {
            var options = Utils.GetOptions(nameof(GetUserPastTripsWrongParams_Should));
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

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.GetUserPastTripsAsync("gogi"));
            }
        }
    }
}
