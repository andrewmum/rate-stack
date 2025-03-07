using rate_app.Core.DTOs;
using rate_app.Core.Entities;
using rate_it_api.Core.Entities;
using rate_it_api.Infra.Repositories;

namespace rate_it_api.Core.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IExternalItemService _externalItemService;
        public RatingService(IRatingRepository rating, IExternalItemService externalItem )
        {
            _ratingRepository = rating;
            _externalItemService = externalItem;
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
        public async Task<IEnumerable<UserRatingWithItemDto>> GetUserRatingsWithItemsAsync(string userId)
        {
            var ratings = await _ratingRepository.GetUserRatingsWithItemsAsync(userId);
            var ratingWithDetails = new List<UserRatingWithItemDto>();
            foreach(var rating in ratings)
            {
                var r = new UserRatingWithItemDto
                {
                    RatingId = rating.Id,
                    Score = rating.Score,
                    Review = rating.Review,
                    CreatedAt = rating.CreatedAt,
                    ItemId = rating.ItemId,
                    ExternalID = rating.Item.ExternalID,
                    Name = rating.Item.Name ?? string.Empty,
                    Category = rating.Item.Category.ToString(),
                    Description = rating.Item.Description,
                };
                if(!string.IsNullOrEmpty(rating.Item.ExternalID))
                {
                    if(rating.Item.Category == Category.Book)
                    {
                        var bookDetails = await _externalItemService.FetchBookDetailsAsync(rating.Item.ExternalID);
                        r.Item = bookDetails;
                    }else if(rating.Item.Category == Category.Resturaunt)
                    {
                        var placeDetails = await _externalItemService.FetchPlaceDetails(rating.Item.ExternalID);
                        r.Item = placeDetails;
                    }

                }
                ratingWithDetails.Add(r);
            }

            return ratingWithDetails;

        }
    }
}
