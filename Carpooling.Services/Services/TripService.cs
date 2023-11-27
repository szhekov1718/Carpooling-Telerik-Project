using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Enums;
using Carpooling.Data.Models;
using Carpooling.Services.Exceptions;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.EntityFrameworkCore;

namespace Carpooling.Services
{
    public class TripService : ITripService
    {
        private readonly CarpoolingContext dbContext;
        private readonly IUserService userService;

        public TripService(CarpoolingContext carpoolingContext, IUserService userService)
        {
            this.dbContext = carpoolingContext;
            this.userService = userService;
        }

        public async Task<TripDTO> CreateAsync(TripDTO tripDTO, Guid driverId)
        {
            if(tripDTO == null)
            {
                throw new EntityNotFound(ExceptionMessages.EntityNotFound);
            }

            if(tripDTO.Departure < DateTime.Now)
            {
                throw new InvalidDepartureTimeException(ExceptionMessages.InvalidDeparture);
            }

            if(tripDTO.FreeSpots < 1)
            {
                throw new ArgumentException(ExceptionMessages.InvalidFreeSpotsNumber);
            }

            var trip = new Trip
            {
                Departure = tripDTO.Departure,
                StartDestination = tripDTO.StartDestination,
                EndDestination = tripDTO.EndDestination,
                FreeSpots = tripDTO.FreeSpots,
                DriverId = driverId
            };

            var driver = await this.userService.GetUserByIdAsync(driverId);
            driver.Trips.Add(trip);

            if(this.dbContext.UserRoles.Where(ur => ur.UserId == driver.Id).Count() == 1)
            {
                driver.UserRoles.Add(new UserRole
                {
                    UserId = driver.Id,
                    RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442142")
                });
            }

            await this.dbContext.Trips.AddAsync(trip);
            await this.dbContext.SaveChangesAsync();

            return new TripDTO(trip);
        }

        public async Task<bool> DeleteAsync(Guid tripId)
        {
            var trip = await this.FindTripByIdAsync(tripId);
            trip.IsDeleted = true;
            await this.dbContext.SaveChangesAsync();

            return trip.IsDeleted;
        }

        public async Task<TripDTO> UpdateAsync(TripDTO updateTrip, Guid tripId)
        {
            if(updateTrip == null)
            {
                throw new EntityNotFound(ExceptionMessages.EntityNotFound);
            }

            if(updateTrip.Departure < DateTime.Now)
            {
                throw new InvalidDepartureTimeException(ExceptionMessages.InvalidDeparture);
            }

            if(updateTrip.FreeSpots < 1)
            {
                throw new ArgumentException(ExceptionMessages.InvalidFreeSpotsNumber);
            }

            var trip = await this.FindTripByIdAsync(tripId);
            trip.Departure = updateTrip.Departure;
            trip.StartDestination = updateTrip.StartDestination ?? trip.StartDestination;
            trip.EndDestination = updateTrip.EndDestination ?? trip.EndDestination;
            trip.FreeSpots = updateTrip.FreeSpots;

            await this.dbContext.SaveChangesAsync();
            return new TripDTO(trip);
        }

        private async Task<Trip> FindTripByIdAsync(Guid tripId)
        {
            var trip = await this.dbContext.Trips
                                           .Include(t => t.Driver)
                                           .Include(t => t.TripCandidates)
                                           .ThenInclude(tc => tc.Passanger)
                                           .Where(t => t.IsDeleted == false)
                                           .FirstOrDefaultAsync(t => t.Id == tripId) ?? throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);

            return trip;
        }

        public async Task<TripDTO> ApplyForTripAsync(string username, Guid tripId)
        {
            var appliedTrip = await this.GetTripAsync(tripId);

            var user = await this.userService.GetUserByUserNameAsync(username) ?? throw new UserNotExistException(ExceptionMessages.InvalidUser);

            if(user.IsBlocked)
            {
                throw new AuthorisationException(ExceptionMessages.UserBlocked);
            }

            if(await this.dbContext.TripCandidates.Where(tc => tc.TripId == tripId && tc.UserId == user.Id).FirstOrDefaultAsync() != null)
            {
                throw new AuthorisationException(ExceptionMessages.TripCandidacyExist);
            }

            var application = new TripCandidate
            {
                UserId = user.Id,
                TripId = tripId,
                DriverId = appliedTrip.DriverId
            };

            await dbContext.TripCandidates.AddAsync(application);
            await this.dbContext.SaveChangesAsync();

            return new TripDTO(appliedTrip);
        }

        public async Task DriverCancelTrip(Guid tripId)
        {
            var trip = await this.GetTripAsync(tripId);

            if(trip.Departure <= DateTime.Now)
            {
                throw new ArgumentException(ExceptionMessages.InvalidAction);
            }

            await this.dbContext.TripCandidates.Where(tc => tc.TripId == tripId && tc.IsApproved == true).ForEachAsync(tc => tc.IsApproved = false);
            trip.TravelStatus = TravelStatus.Canceled;
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<Trip> GetTripAsync(Guid tripId)
        {
            var trip = await FindTripByIdAsync(tripId);

            return trip;
        }

        public async Task<ICollection<Trip>> GetAllTripsAsync()
        {
            var result = await this.dbContext.Trips
                .ToListAsync();

            if(!result.Any())
            {
                throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrips);
            }

            return result;
        }

        public async Task<ICollection<TripDTO>> GetAllAvailableTripsAsync()
        {
            var result = await this.dbContext.Trips
                .Where(t => t.Departure > DateTime.Now.Date && t.FreeSpots > 0)
                .Select(t => new TripDTO(t))
                .ToListAsync();

            if(!result.Any())
            {
                throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrips);
            }

            return result;
        }

        public async Task<ICollection<Trip>> AvailableTripsAsync()
        {
            var result = await this.dbContext.Trips
                .Where(t => t.Departure > DateTime.Now.Date && t.FreeSpots > 0)
                .ToListAsync();

            if(!result.Any())
            {
                throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrips);
            }

            return result;
        }

        public async Task<List<TripDTO>> FilterTripsByStartDestinationAsync(string startDestination, string allOrAvailableTrips)
        {
            if(allOrAvailableTrips == "all")
            {
                var result = await this.dbContext.Trips
                .Where(t => t.IsDeleted == false && t.StartDestination == startDestination)
                .Select(t => new TripDTO(t))
                .ToListAsync();

                if(!result.Any())
                {
                    throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
                }

                return result;
            }
            if(allOrAvailableTrips == "available")
            {
                var result = await this.dbContext.Trips
                .Where(t => t.IsDeleted == false && t.StartDestination == startDestination && t.FreeSpots > 0 && t.Departure > DateTime.Now.Date)
                .Select(t => new TripDTO(t))
                .ToListAsync();

                if(!result.Any())
                {
                    throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
                }

                return result;
            }

            throw new ArgumentException(ExceptionMessages.InvalidFilter);
        }

        public async Task<List<TripDTO>> FilterTripsByEndDestinationAsync(string endDestination, string allOrAvailableTrips)
        {
            if(allOrAvailableTrips == "all")
            {
                var result = await this.dbContext.Trips
                .Where(t => t.IsDeleted == false && t.EndDestination == endDestination)
                .Select(t => new TripDTO(t))
                .ToListAsync();

                if(!result.Any())
                {
                    throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
                }

                return result;
            }
            if(allOrAvailableTrips == "available")
            {
                var result = await this.dbContext.Trips
                .Where(t => t.IsDeleted == false && t.EndDestination == endDestination && t.FreeSpots > 0 && t.Departure < DateTime.Now.Date)
                .Select(t => new TripDTO(t))
                .ToListAsync();

                if(!result.Any())
                {
                    throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
                }

                return result;
            }

            throw new ArgumentException(ExceptionMessages.InvalidFilter);
        }

        public async Task<List<TripDTO>> FilterTripsByDepartureAsync(DateTime departure, string allOrAvailableTrips)
        {
            if(allOrAvailableTrips == "all")
            {
                var result = await this.dbContext.Trips
                .Where(t => t.IsDeleted == false && t.Departure == departure)
                .Select(t => new TripDTO(t))
                .ToListAsync();

                if(!result.Any())
                {
                    throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
                }

                return result;
            }
            if(allOrAvailableTrips == "available")
            {
                var result = await this.dbContext.Trips
                .Where(t => t.IsDeleted == false && t.Departure == departure && t.FreeSpots > 0 && t.Departure < DateTime.Now.Date)
                .Select(t => new TripDTO(t))
                .ToListAsync();

                if(!result.Any())
                {
                    throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
                }

                return result;
            }

            throw new ArgumentException(ExceptionMessages.InvalidFilter);
        }

        public async Task<List<TripDTO>> GetUserPastTripsAsync(string username)
        {
            var user = await this.userService.GetUserByUserNameAsync(username);
            var result = await this.dbContext.Users.Include(u => u.Trips)
                                             .Where(u => u.Username == username)
                                             .SelectMany(u => u.Trips)
                                             .Where(t => t.Departure < DateTime.Now.Date)
                                             .Select(t => new TripDTO(t))
                                             .ToListAsync();

            if(!result.Any())
            {
                throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
            }

            return result;
        }

        public async Task<List<TripDTO>> GetUserFutureTripsAsync(string username)
        {
            var user = await this.userService.GetUserByUserNameAsync(username);
            var result = await this.dbContext.Users.Include(u => u.Trips)
                                             .Where(u => u.Username == username)
                                             .SelectMany(u => u.Trips)
                                             .Where(t => t.Departure > DateTime.Now.Date)
                                             .Select(t => new TripDTO(t))
                                             .ToListAsync();

            if(!result.Any())
            {
                throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
            }

            return result;
        }

        public async Task<IList<TripDTO>> GetTripsByUserRoleAsync(string username, string role)
        {
            var user = await this.userService.GetUserByUserNameAsync(username);

            if(user == null)
            {
                throw new UserNotExistException(ExceptionMessages.InvalidUser);
            }

            if(role.ToLower() == "driver")
            {
                var trips = await this.dbContext.Trips.Where(t => t.DriverId == user.Id)
                                                .Select(t => new TripDTO(t))
                                                .ToListAsync();

                if(!trips.Any())
                {
                    throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
                }

                return trips;
            }
            if(role.ToLower() == "passenger")
            {
                var trips = await this.dbContext.Trips.Include(t => t.TripCandidates)
                                                .SelectMany(t => t.TripCandidates.Where(t => t.UserId == user.Id && t.IsApproved == true))
                                                .Select(t => t.Trip)
                                                .Select(t => new TripDTO(t))
                                                .ToListAsync();

                if(!trips.Any())
                {
                    throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
                }

                return trips;
            }

            if(role.ToLower() == "all")
            {
                var tripsLikeDriver = await this.dbContext.Trips.Where(t => t.DriverId == user.Id)
                                                .Select(t => new TripDTO(t))
                                                .ToListAsync();

                var tripsLikePassenger = await this.dbContext.Trips.Include(t => t.TripCandidates)
                                                .SelectMany(t => t.TripCandidates.Where(t => t.UserId == user.Id && t.IsApproved == true))
                                                .Select(t => t.Trip)
                                                .Select(t => new TripDTO(t))
                                                .ToListAsync();

                tripsLikeDriver.AddRange(tripsLikePassenger);

                if(!tripsLikeDriver.Any())
                {
                    throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrips);
                }

                return tripsLikeDriver;
            }

            throw new ArgumentException(ExceptionMessages.InvalidRoleForTrip);
        }

        public async Task<string> AddTripCommentAsync(Guid driverId, Guid tripId, string comment)
        {
            var trip = await this.dbContext.Trips.Where(t => t.Id == tripId && t.DriverId == driverId).FirstOrDefaultAsync() ??
                throw new EntityNotFound(ExceptionMessages.EntityNotFound);

            var tripComment = new TripComment
            {
                Comment = comment,
                TripId = tripId,
                IsDeleted = false
            };

            await this.dbContext.AddAsync(tripComment);
            await this.dbContext.SaveChangesAsync();

            return tripComment.Comment;
        }
    }
}


