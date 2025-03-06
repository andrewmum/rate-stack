using Microsoft.AspNetCore.Mvc;
using rate_it_api.Core.DTOs;
using rate_it_api.Core.Services;

namespace rate_it_api.Api.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [HttpGet]
        public async Task<IActionResult> SearchItems([FromQuery] string? category)
        {
            var items = await _itemService.SearchItemsAsync(category);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ItemDto itemDto)
        {
            var item = await _itemService.CreateItemAsync(itemDto.Name, 0, itemDto.Description);
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }
    }
}
