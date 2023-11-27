using System;
using System.Collections.Generic;
using Carpooling.Data.Enums;
using Carpooling.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Carpooling.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var roles = new List<Role>()
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
                }
            };

            var users = new List<User>()
            {
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442143"),
                    Username ="Stenlyto",
                    Password = "cd5392bb8db39640ea5ee097ce0a832f8e506cc1c0ca685812575acd83ee5a8c", //Stenlyto*12
                    FirstName = "Stanislav",
                    LastName = "Simeonov",
                    Email = "stenlyto@abv.bg",
                    PhoneNumber = "0854545454"
                },
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442141"),
                    Username ="Mirko",
                    Password = "b6bd0cb173bd0081e448a2400b622f75ddb8cc36c17edebf0ba3d1a08fa164c0", //Mirko*12
                    FirstName = "Stanimir",
                    LastName = "Ivanov",
                    Email = "miro44@abv.bg",
                    PhoneNumber = "0864646464"
                },
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442145"),
                    Username ="Pepi",
                    Password = "428ed7ff0b40a507792b58445998623722c87818bcbdec0869010e5340df6fb0", //Pepi*22
                    FirstName = "Petko",
                    LastName = "Mitev",
                    Email = "pepi14@abv.bg",
                    PhoneNumber = "0812122112"
                },
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442153"),
                    Username ="gogi",
                    Password = "2c4950b025be3ac9215b130365e974a9a400fb8b5984f2f31db747b86b962ffd", //Gogi*12
                    FirstName = "Georgi",
                    LastName = "Mitev",
                    Email = "gogi@abv.bg",
                    PhoneNumber = "0854545459",
                },
                new User
                {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442233"),
                    Username ="tisho",
                    Password = "d54d0553dd78e8aea5db95ffa176de9019bce947fc02aecca916378ae216a4e3", //Tisho*12
                    FirstName = "Todor",
                    LastName = "Todorov",
                    Email = "tisho@abv.bg",
                    PhoneNumber = "0854545453",
                    IsAdmin = true
                },
            };

            var userRoles = new List<UserRole>()
            {
                new UserRole
                {
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143"),
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442144")
                },
                new UserRole
                {
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143"),
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442142")
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

            var trips = new List<Trip>()
            {
                 new Trip
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    StartDestination = "Sofia",
                    EndDestination = "Plovdiv",
                    Departure = new DateTime(2021, 12, 21),
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
                 }
            };

            var tripCandidates = new List<TripCandidate>()
            {
                 new TripCandidate
                {
                    Id = Guid.Parse("143b692d-330e-405d-a019-c3d728442149"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442141"), // 1st user - rejected from 1st trip
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143")//Stenlyto
                },
                new TripCandidate
                {
                    Id = Guid.Parse("143b692d-330e-405d-a019-c3d728442150"),   // 1st user - aproved from 2nd trip
                    IsApproved = true,
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442141"),
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145") //Pepi
                },
                new TripCandidate
                {
                    Id = Guid.Parse("143b692d-330e-405d-a019-c3d728442151"),
                    IsApproved = true,
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442153"),  // 2nd user - 1st trip approved
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143") //Stenlyto
                },
                new TripCandidate
                {
                    Id = Guid.Parse("143b692d-330e-405d-a019-c3d728442152"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442153"),  // 2nd user - 2nd trip rejected
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145") //Pepi
                },
                new TripCandidate
                {
                    Id = Guid.Parse("143b692d-330e-405d-a019-c3d828442153"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442233"),  //tisho
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),  // 3rd user -  rejected from 1st trip
                    DriverId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143") //Stenlyto
                }
            };

            var feedbacks = new List<Feedback>()
            {
                new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442151"),
                    Rating = 5,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442142"), //Driver
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442143") //Stenlyto
                 },
                 new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442152"),
                    Rating = 4,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442144"), //Passenger
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442141") //Mirko
                 },
                 new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442153"),
                    Rating = 3,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442142"), //Driver
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442145")  //Pepi
                 },
                 new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442154"),
                    Rating = 2,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442144"), //Passenger
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442141") //Mirko
                 },
                 new Feedback
                 {
                    Id = Guid.Parse("943b692d-330e-405d-a019-c3d728442155"),
                    Rating = 1,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442144"), //Passenger
                    TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    UserId = Guid.Parse("943b692d-330e-405d-a019-c3d728442153") //gogi
                 }
            };

            modelBuilder.Entity<Role>().HasData(roles);
            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Trip>().HasData(trips);
            modelBuilder.Entity<TripCandidate>().HasData(tripCandidates);
            modelBuilder.Entity<Feedback>().HasData(feedbacks);
            modelBuilder.Entity<UserRole>().HasData(userRoles);
            //modelBuilder.Entity<User>().HasQueryFilter(u => EF.Property<bool>(u, "IsDeleted") == false); // TODO
        }
    }
}
