using Carpooling.Data.Models;

namespace Carpooling.MVC.Models
{
    public class TripViewModel : Trip
    {
        public TripViewModel()
        {

        }
        public TripViewModel(Trip trip)
        {
            this.Id = trip.Id;
            this.StartDestination = trip.StartDestination;
            this.EndDestination = trip.EndDestination;
            this.Departure = trip.Departure;
            this.FreeSpots = trip.FreeSpots;
            this.TravelStatus = trip.TravelStatus;
            this.TripCandidates = trip.TripCandidates;
            this.Driver = trip.Driver;
            this.DriverId = trip.DriverId;
        }
    }
}
