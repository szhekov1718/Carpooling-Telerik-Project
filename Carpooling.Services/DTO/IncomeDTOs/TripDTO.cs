using Carpooling.Data.Models;

namespace Carpooling.Services
{
    public class TripDTO : Trip
    {
        public TripDTO()
        {

        }
        public TripDTO(Trip trip)
        {
            this.Id = trip.Id;
            this.StartDestination = trip.StartDestination;
            this.EndDestination = trip.EndDestination;
            this.Departure = trip.Departure;
            this.FreeSpots = trip.FreeSpots;
            this.TravelStatus = trip.TravelStatus;
        }
    }
}