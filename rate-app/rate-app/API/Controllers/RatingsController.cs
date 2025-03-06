using Microsoft.AspNetCore.Mvc;
using rate_it_api.Core.DTOs;
using rate_it_api.Core.Services;

namespace rate_it_api.Api.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("item/itemId")]
        public async Task<IActionResult> GetRatingsForItem(int itemId)
        {
            var ratings = await _ratingService.GetRatingsForItemAsync(itemId);
            return Ok(ratings);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRating([FromBody] RatingDto ratingDto)
        {
            var rating = await _ratingService.SubmitRatingAsync(ratingDto.UserId, ratingDto.ItemId, ratingDto.Score, ratingDto.Review);
            return CreatedAtAction(nameof(GetRatingsForItem), new { itemId = ratingDto.ItemId }, rating);
        }
    }
}
