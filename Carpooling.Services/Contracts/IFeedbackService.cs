using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpooling.Data.Enums;
using Carpooling.Data.Models;
using Carpooling.Services.DTO.IncomeDTOs;

namespace Carpooling.Services.Contracts
{
    public interface IFeedbackService
    {
        Task<FeedbackDTO> CreateFeedbackAsync(CreateFeedbackDTO feedbackDTO);
        Task<Feedback> GetFeedbackAsync(Guid feedbackId);
        Task EditFeedbackAsync(Feedback data);
        Task UpdateDriverPassengerRating(Guid tripId, TravelRole creatorRole, int rating);
        ICollection<UserRatingDTO> TopTenDirverPassengerRatings(TravelRole role);
        Task<bool> DeleteAsync(Guid userId, Guid feedbackId);
        Task<ICollection<Feedback>> ListAllUserFeedbacksAsync(Guid id);
        Task<IList<Feedback>> ListAllFeedbacksForUserAsync(Guid id);
        Task<ICollection<FeedbackDTO>> ListAllFeedbacksAsync();
        Task<ICollection<UserRatingDTO>> AllRatingsCountAsync();

    }
}
