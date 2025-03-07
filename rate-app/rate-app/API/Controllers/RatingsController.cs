using Microsoft.AspNetCore.Mvc;
using rate_app.Core.Entities;
using rate_it_api.Core.DTOs;
using rate_it_api.Core.Entities;
using rate_it_api.Core.Services;

namespace rate_it_api.Api.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IItemService _itemService;
        private readonly ICurrentUserService _currentUser;

        public RatingsController(IRatingService ratingService, IItemService itemService, ICurrentUserService currentUser)
        {
            _ratingService = ratingService;
            _itemService = itemService;
            _currentUser = currentUser;
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
            if (ratingDto.ItemId == null && ratingDto.ExternalItemId == null)
                return BadRequest("no item id");
            Rating ratings = await _ratingService.GetRatingsForItemAsync((int)ratingDto.ItemId);
            Item item = ratings == null ? await _itemService.CreateExternalItemAsync(ratingDto.ExternalItemId, ratingDto.Category) : null;
            try
            {
                var rating = await _ratingService.SubmitRatingAsync(_currentUser.GetUserId(), item.Id, ratingDto.Score, ratingDto.Review);
                return CreatedAtAction(nameof(GetRatingsForItem), new { itemId = item.Id }, rating);
            }
            catch(Exception ex)
            {
                Console.Write("error", ex);
                return BadRequest();
            }
        }

        [HttpGet("my-ratings")]
        public async Task<IActionResult> GetMyRatings()
        {
            var userId = _currentUser.GetUserId();
            var ratings = await _ratingService.GetUserRatingsWithItemsAsync(userId);
            return Ok(ratings);
        }
        

    }
}
