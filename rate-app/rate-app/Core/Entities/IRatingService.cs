using rate_app.Core.DTOs;
using rate_it_api.Core.Entities;

namespace rate_it_api.Core.Services
{
    public interface IRatingService

    {
        Task<Rating> GetRatingsForItemAsync(int itemId);
        Task<Rating> SubmitRatingAsync(string userId, int itemId, int score, string review);

        Task<IEnumerable<UserRatingWithItemDto>> GetUserRatingsWithItemsAsync(string userId);

    }
}
