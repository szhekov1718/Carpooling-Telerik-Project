using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Models;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.EntityFrameworkCore;

namespace Carpooling.Services.Services
{
    public class TripCandidateService : ITripCandidateService
    {
        private readonly CarpoolingContext dbContext;
        private readonly IUserService userService;

        public TripCandidateService(CarpoolingContext carpoolingContext, IUserService userService)
        {
            this.dbContext = carpoolingContext;
            this.userService = userService;
        }

        public async Task<TripCandidate> GetTripCandidateAsync(Guid tripCandidacyId)
        {
            var candidacy = await this.dbContext.TripCandidates.Where(tc => tc.Id == tripCandidacyId).FirstOrDefaultAsync();

            if(candidacy == null)
            {
                throw new EntityNotFound(ExceptionMessages.EntityNotFound);
            }

            return candidacy;
        }

        public async Task<TripCandidate> GetTripCandidateAsync(Guid userId, Guid tripId)
        {
            var candidacy = await this.dbContext.TripCandidates.Where(tc => tc.UserId == userId && tc.TripId == tripId)
                                                               .FirstOrDefaultAsync();

            if(candidacy == null)
            {
                throw new EntityNotFound(ExceptionMessages.EntityNotFound);
            }

            return candidacy;
        }

        public async Task<ICollection<TripCandidate>> GetAllTripCandidatesForTripAsync(Guid id)
        {
            var candidates = await this.dbContext.TripCandidates.Include(tc => tc.Passanger)
                                                                .ThenInclude(p => p.UserRoles)
                                                                .Include(tc => tc.Passanger)
                                                                .Where(tc => tc.TripId == id && tc.IsDeleted == false)
                                                                .ToListAsync();

            if(!candidates.Any())
            {
                throw new EntityNotFound(ExceptionMessages.NoTripCandidacy);
            }

            return candidates;
        }

        public async Task<ICollection<TripCandidate>> GetAllTripCandidatesAsync()
        {
            var candidates = await this.dbContext.TripCandidates.ToListAsync();

            if(!candidates.Any())
            {
                throw new EntityNotFound(ExceptionMessages.EntityNotFound);
            }

            return candidates;
        }

        public async Task<bool> PassengerDeclineTripCandidacyAsync(string username, Guid tripCandidacyId)
        {
            var user = await this.userService.GetUserByUserNameAsync(username);
            if(user == null)
            {
                throw new UserNotExistException(ExceptionMessages.InvalidUser);
            }

            var tripCandidate = await this.dbContext.TripCandidates
                                                    .Where(t => t.Id == tripCandidacyId && t.UserId == user.Id)
                                                    .FirstOrDefaultAsync();

            if(tripCandidate == null)
            {
                throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
            }

            tripCandidate.IsDeleted = true;
            tripCandidate.IsApproved = false;
            this.dbContext.Trips.Where(t => t.Id == tripCandidate.TripId).Select(t => t.FreeSpots + 1);
            await this.dbContext.SaveChangesAsync();

            return tripCandidate.IsDeleted;
        }

        public async Task<List<UserDTO>> ListTripApprovedCandidatesAsync(Guid tripId)
        {
            var trip = await this.dbContext.Trips.Where(t => t.Id == tripId).FirstOrDefaultAsync();

            var candidates = await this.dbContext.TripCandidates
                                           .Include(t => t.Passanger)
                                           .Where(t => t.TripId == tripId && t.IsApproved == true)
                                           .Select(t => t.Passanger)
                                           .Select(u => new UserDTO(u))
                                           .ToListAsync();

            if(!candidates.Any())
            {
                throw new NoApprovedCandidates(ExceptionMessages.NoApprovedCandidates);
            }

            return candidates;
        }

        public async Task<List<UserDTO>> ListTripRejectedCandidatesAsync(Guid tripId)
        {
            var trip = await this.dbContext.Trips.Where(t => t.Id == tripId).FirstOrDefaultAsync();

            var result = await this.dbContext.TripCandidates
                                       .Include(t => t.Passanger)
                                       .Where(t => t.TripId == tripId && t.IsApproved == false)
                                       .Select(t => t.Passanger)
                                       .Select(u => new UserDTO(u))
                                       .ToListAsync();
            if(!result.Any())
            {
                throw new ArgumentException(ExceptionMessages.NoRejectedCandidates);
            }

            return result;
        }

        public async Task<bool> ApproveCandidateAsync(Guid candidateId, Guid tripId, Guid driverId)
        {
            var trip = await this.dbContext.Trips.Where(t => t.Id == tripId).FirstOrDefaultAsync();

            if(trip == null)
            {
                throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
            }

            if(driverId != trip.DriverId)
            {
                throw new AuthorisationException(ExceptionMessages.InvalidAction);
            }

            if(trip.Departure.Date <= DateTime.Now.Date)
            {
                throw new ArgumentException(ExceptionMessages.ApproveCandidateInDepartureDay);
            }
            var candidate = await this.dbContext.TripCandidates
                .Where(tc => tc.UserId == candidateId && tc.TripId == tripId && tc.DriverId == driverId)
                .FirstOrDefaultAsync() ?? throw new TravelPassengerDoesNotExistException(ExceptionMessages.InvalidCandidate);

            candidate.IsApproved = true;
            trip.FreeSpots--;
            await this.dbContext.SaveChangesAsync();

            return candidate.IsApproved;
        }

        public async Task<bool> RejectCandidateAsync(Guid candidateId, Guid tripId, Guid driverId)
        {
            var trip = await this.dbContext.Trips.Where(t => t.Id == tripId).FirstOrDefaultAsync();

            if(trip == null)
            {
                throw new TravelDoesNotExistException(ExceptionMessages.InvalidTrip);
            }

            if(driverId != trip.DriverId)
            {
                throw new AuthorisationException(ExceptionMessages.InvalidAction);
            }

            if(trip.Departure.Date <= DateTime.Now.Date)
            {
                throw new ArgumentException(ExceptionMessages.RejectCandidateInDepartureDay);
            }

            var candidate = await this.dbContext.TripCandidates
                .Where(tc => tc.UserId == candidateId && tc.TripId == tripId && tc.DriverId == driverId)
                .FirstOrDefaultAsync() ?? throw new TravelPassengerDoesNotExistException(ExceptionMessages.InvalidCandidate);

            candidate.IsApproved = false;
            await this.dbContext.SaveChangesAsync();

            return candidate.IsApproved;
        }
    }
}
