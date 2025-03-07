using rate_app.Core.DTOs;
using rate_it_api.Core.DTOs;

namespace rate_app.Core.Entities
{
    public interface IExternalItemService
    {
        Task<ItemDto> FetchBookDetailsAsync(string bookId);
        Task<ItemDto> FetchPlaceDetails(string placeId);
    }
}
