using rate_it_api.Core.Entities;
using rate_it_api.Infra.Repositories;

namespace rate_it_api.Core.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        public RatingService(IRatingRepository rating)
        {
            _ratingRepository = rating;
        }
        public async Task<Rating> GetRatingsForItemAsync(int itemId)
        {
            return await _ratingRepository.GetRatingsForItemAsync(itemId);
        }
        public async Task<Rating> SubmitRatingAsync(string userId, int itemId, int score, string review)
        {
            var rating = new Rating { UserId = userId, ItemId = itemId, Score = score, Review = review };
            return await _ratingRepository.SubmitRatingAsync(rating);
        }
    }
}
