using System;

namespace Carpooling.Services.DTO.IncomeDTOs
{
    public class TripCandidateDTO
    {
        public TripCandidateDTO()
        {

        }
        public TripCandidateDTO(Guid userId, Guid tripId, Guid driverId)
        {
            UserId = userId;
            TripId = tripId;
            DriverId = driverId;
        }
        public Guid UserId { get; set; }
        public Guid TripId { get; set; }
        public Guid DriverId { get; set; }
    }
}
