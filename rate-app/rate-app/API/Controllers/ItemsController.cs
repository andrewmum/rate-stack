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
        [HttpGet]
        [Route("allitems")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

    }
}
