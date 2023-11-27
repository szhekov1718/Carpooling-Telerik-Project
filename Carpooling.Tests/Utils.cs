using System;
using System.Collections.Generic;
using Carpooling.Data;
using Carpooling.Data.Enums;
using Carpooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests
{
    [TestClass]
    public class Utils
    {
        public static DbContextOptions<CarpoolingContext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<CarpoolingContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        public static IEnumerable<Role> SeedRoles()
        {
            return new List<Role>()
            {
                new Role
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442142"),
                    TravelRole = TravelRole.Driver
                },
                new Role
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442144"),
                    TravelRole = TravelRole.Passenger
                },
                 new Role
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442145"),
                    TravelRole = TravelRole.Driver
                },
                  new Role
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    TravelRole = TravelRole.Passenger
                },
                   new Role
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442147"),
                    TravelRole = TravelRole.Passenger
                },
                   new Role
                   {
                       Id = Guid.Parse("943b692d-330e-405d-a019-c3d728445689"),
                       TravelRole = TravelRole.Driver
                   }
            };
        }
        public static IEnumerable<User> SeedUsers()
        {
            return new List<User>()
            {
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442143"),
                    Username ="Stenlyto",
                    Password = "cd5392bb8db39640ea5ee097ce0a832f8e506cc1c0ca685812575acd83ee5a8c",
                    FirstName = "Stanislav",
                    LastName = "Simeonov",
                    Email = "stenlyto@abv.bg",
                    IsAdmin = true,
                    PhoneNumber = "0854545454"
                },
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442141"),
                    Username ="Mirko",
                    Password = "Mirko*12",
                    FirstName = "Stanimir",
                    LastName = "Ivanov",
                    Email = "miro44@abv.bg",
                    IsBlocked = true,
                    PhoneNumber = "0864646464"
                },
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442145"),
                    Username ="Pepi",
                    Password = "Pepi*22",
                    FirstName = "Petko",
                    LastName = "Mitev",
                    Email = "pepi14@abv.bg",
                    PhoneNumber = "0812122112"
                },
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442153"),
                    Username ="gogi",
                    Password = "Gogi*12",
                    FirstName = "Georgi",
                    LastName = "Mitev",
                    Email = "gogi@abv.bg",
                    PhoneNumber = "0854545459"
                },
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442233"),
                    Username ="tisho",
                    Password = "Tisho*12",
                    FirstName = "Todor",
                    LastName = "Todorov",
                    Email = "tisho@abv.bg",
                    PhoneNumber = "0854545453"
                },
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728445689"),
                    Username ="geri",
                    Password = "Geri*16",
                    FirstName = "Gergana",
                    LastName = "Azova",
                    Email = "geri@abv.bg",
                    PhoneNumber = "0899279132",
                    IsBlocked = true
                },
            };
        }

        public static IEnumerable<UserRole> SeedUserRoles()
        {
            return new List<UserRole>()
            {
                new UserRole
                {
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143"),
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442142")
                },
                new UserRole
                {
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143"),
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442144")
                },
                new UserRole
                {
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145"),
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442142")
                },
                new UserRole
                {
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145"),
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442144")
                }
            };
        }
        public static IEnumerable<Trip> SeedTrips()
        {
            return new List<Trip>()
            {
                 new Trip
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    StartDestination = "Sofia",
                    EndDestination = "Plovdiv",
                    Departure = new DateTime(2021, 1, 1),
                    FreeSpots = 3,
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143"),
                 },
                 new Trip
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    StartDestination = "Plovdiv",
                    EndDestination = "Sofia",
                    Departure = new DateTime(2021, 12, 15),
                    FreeSpots = 3,
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                 },
                 new Trip
                 {
                    Id = Guid.Parse("943b692d-220e-405d-a019-c3d728442146"),
                    StartDestination = "Burgas",
                    EndDestination = "Pleven",
                    Departure = new DateTime(2021, 11, 10),                     // trip with past date for test
                    FreeSpots = 2,
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728445689")
                 }
            };
        }
        public static IEnumerable<TripCandidate> SeedTripCandidates()
        {
            return new List<TripCandidate>()
            {
                new TripCandidate
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442149"),
                    IsApproved = false,
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442141"), // 1st user - rejected from 1st trip
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143")
                },
                new TripCandidate
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728443149"),
                    IsApproved = true,
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143"), // 1st user - rejected from 1st trip
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                },
                new TripCandidate
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442150"),   // 1st user - approved from 2nd trip
                    IsApproved = true,
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442141"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                },
                new TripCandidate
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442151"),
                    IsApproved = true,
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442153"),  // 2nd user - 1st trip approved
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143")
                },
                new TripCandidate
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442152"),
                    IsApproved = false,
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442153"),  // 2nd user - 2nd trip rejected
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                },
                new TripCandidate
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442153"),
                    IsApproved = false,
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442233"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"), // 3rd user -  rejected from 1st trip
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143")
                },
                new TripCandidate
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442666"),
                    IsApproved = false,
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442659"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"), // 3rd user - rejected from 2nd trip
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                },
            };
        }

        public static IEnumerable<Feedback> SeedFeedbacks()
        {
            return new List<Feedback>()
            {
                 new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442151"),
                    Rating = 5,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442142"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143")
                 },
                 new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442152"),
                    Rating = 4,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442144"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442141")
                 },
                 new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442153"),
                    Rating = 3,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")
                 },
                 new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442154"),
                    Rating = 2,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442141")
                 },
                 new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442155"),
                    Rating = 1,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442147"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442153")
                 }
            };
        }

        public static IEnumerable<TripComment> SeedTripComments()
        {
            return new List<TripComment>()
            {
                new TripComment()
                {
                    Id = Guid.Parse("942b692d-330e-405d-a019-c3d728442147"),
                    Comment = "The trip was perfect.",
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146")
                },
                new TripComment()
                {
                    Id = Guid.Parse("941b692d-330e-405d-a019-c3d728442147"),
                    Comment = "The trip was not pleasant, because the driver was smoking in the car.",
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148")
                }
            };
        }
    }






































}

