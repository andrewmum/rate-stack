using rate_it_api.Core.Entities;

namespace rate_it_api.Infra.Repositories
{
    public interface IRatingRepository
    {
        Task<Rating> GetRatingsForItemAsync(int itemId);
        Task<Rating> SubmitRatingAsync(Rating rating);
        Task<IEnumerable<Rating>> GetUserRatingsWithItemsAsync(string userId);

    }
}
