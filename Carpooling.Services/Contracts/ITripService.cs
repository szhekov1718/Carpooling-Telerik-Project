using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpooling.Data.Models;

namespace Carpooling.Services
{
    public interface ITripService
    {
        Task<TripDTO> CreateAsync(TripDTO tripDTO, Guid driverId);
        Task<bool> DeleteAsync(Guid tripId);
        Task<TripDTO> UpdateAsync(TripDTO updateTrip, Guid tripId);
        Task<Trip> GetTripAsync(Guid tripId);
        Task<ICollection<Trip>> GetAllTripsAsync();
        Task<ICollection<TripDTO>> GetAllAvailableTripsAsync();
        Task<ICollection<Trip>> AvailableTripsAsync();
        Task<TripDTO> ApplyForTripAsync(string username, Guid tripId);
        Task DriverCancelTrip(Guid tripId);
        Task<List<TripDTO>> FilterTripsByStartDestinationAsync(string startDestination, string allOrAvailableTrips);
        Task<List<TripDTO>> FilterTripsByEndDestinationAsync(string endDestination, string allOrAvailableTrips);
        Task<List<TripDTO>> FilterTripsByDepartureAsync(DateTime departure, string allOrAvailableTrips);
        Task<List<TripDTO>> GetUserPastTripsAsync(string username);
        Task<List<TripDTO>> GetUserFutureTripsAsync(string username);
        Task<IList<TripDTO>> GetTripsByUserRoleAsync(string username, string role);
        Task<string> AddTripCommentAsync(Guid driverId, Guid tripId, string comment);
    }
}
