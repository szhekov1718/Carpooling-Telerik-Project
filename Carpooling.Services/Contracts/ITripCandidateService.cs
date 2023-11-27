using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpooling.Data.Models;
using Carpooling.Services.DTO.IncomeDTOs;

namespace Carpooling.Services.Contracts
{
    public interface ITripCandidateService
    {
        Task<TripCandidate> GetTripCandidateAsync(Guid tripCandidacyId);
        Task<TripCandidate> GetTripCandidateAsync(Guid userId, Guid tripId);
        Task<ICollection<TripCandidate>> GetAllTripCandidatesForTripAsync(Guid id);
        Task<ICollection<TripCandidate>> GetAllTripCandidatesAsync();
        Task<bool> PassengerDeclineTripCandidacyAsync(string username, Guid tripCandidacyId);
        Task<bool> RejectCandidateAsync(Guid candidateId, Guid tripId, Guid driverId);
        Task<bool> ApproveCandidateAsync(Guid candidateId, Guid tripId, Guid driverId);
        Task<List<UserDTO>> ListTripRejectedCandidatesAsync(Guid tripId);
        Task<List<UserDTO>> ListTripApprovedCandidatesAsync(Guid tripId);
    }
}
