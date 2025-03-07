using Microsoft.Extensions.Caching.Memory;
using rate_app.Core.DTOs;
using rate_it_api.Core.DTOs;
using System.Text.Json;

namespace rate_app.Core.Entities
{
    public class ExternalItemService : IExternalItemService
    {
        IMemoryCache _memoryCache;
        IConfiguration _config;
        IHttpClientFactory _httpClientFactory;
        public ExternalItemService(IMemoryCache memoryCache, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ItemDto> FetchPlaceDetails(string placeId)
        {
            if (_memoryCache.TryGetValue($"place_{placeId}", out PlaceDetailsDto details))
                return details;

            try
            {
                var apiKey = _config["GoogleApiKeys:PlacesKey"];
                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.GetAsync($"https://places.googleapis.com/v1/places/{placeId}?fields=id,displayName,editorialSummary,shortFormattedAddress,photos&key={apiKey}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var placeData = JsonSerializer.Deserialize<JsonElement>(content);


                var placeDetails = new PlaceDetailsDto
                {
                    Name = GetNestedJsonPropertyStringValue(placeData, "displayName.text", "No Display Name"),
                    Description = GetNestedJsonPropertyStringValue(placeData, "editorialSummary.text", "No Description Available"),
                    Address = GetJsonPropertyStringValue(placeData, "shortFormattedAddress", "No Address")
                };
                if (placeData.TryGetProperty("photos", out var photosElement) &&
                                   photosElement.ValueKind == JsonValueKind.Array)
                {
                    var photoArray = photosElement.EnumerateArray().ToArray();
                    if (photoArray.Length > 0)
                    {
                        // Get the first photo's name
                        var photoName = GetJsonPropertyStringValue(photoArray[0], "name", string.Empty);
                        if (!string.IsNullOrEmpty(photoName))
                        {
                            placeDetails.Thumbnail =
                                $"https://places.googleapis.com/v1/{photoName}/media?key={apiKey}&maxHeightPx=200&maxWidthPx=200";
                        }
                    }
                }
                _memoryCache.Set($"place_{placeId}", placeDetails, TimeSpan.FromHours(24));
                return placeDetails;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching place details: {ex.Message}");

                return new PlaceDetailsDto
                {
                    Name = "Error fetching place",
                    Description = "Unable to retrieve place details"
                };
            }
        }
        public async Task<ItemDto>  FetchBookDetailsAsync(string bookId)
        {
            if(_memoryCache.TryGetValue($"book_{bookId}", out BookDetailsDto details))
            {
                return details;
            }
            try
            {
                var apiKey = _config["GoogleBooks:ApiKey"];
                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.GetAsync($"https://www.googleapis.com/books/v1/volumes/{bookId}?key={apiKey}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var bookData = JsonSerializer.Deserialize<JsonElement>(content);

                var volumeInfo = bookData.GetProperty("volumeInfo");

                var bookDetails = new BookDetailsDto
                {
                    Title = GetJsonPropertyStringValue(volumeInfo, "title", "Unknown Title"),
                    Authors = GetJsonPropertyStringArray(volumeInfo, "authors", new[] { "Unknown Author" }),
                    Description = GetJsonPropertyStringValue(volumeInfo, "description", ""),
                    PublishedDate = GetJsonPropertyStringValue(volumeInfo, "publishedDate", ""),
                    Categories = GetJsonPropertyStringArray(volumeInfo, "categories", Array.Empty<string>()),
                };
                if (volumeInfo.TryGetProperty("imageLinks", out var imageLinks) && imageLinks.TryGetProperty("thumbnail", out var thumbnail))
                {
                    bookDetails.Thumbnail = thumbnail.GetString();
                }

                _memoryCache.Set($"book_{bookId}", bookDetails, TimeSpan.FromHours(24));

                return bookDetails;
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine($"Error fetching book details for ID {bookId}: {ex.Message}");
                return null;
            }

        }
        private string GetJsonPropertyStringValue(JsonElement element, string propertyName, string defaultValue)
        {
            return element.TryGetProperty(propertyName, out var property) ? property.GetString() ?? defaultValue : defaultValue;
        }

        private int GetJsonPropertyIntValue(JsonElement element, string propertyName, int defaultValue)
        {
            return element.TryGetProperty(propertyName, out var property) && property.TryGetInt32(out var value) ? value : defaultValue;
        }

        private string[] GetJsonPropertyStringArray(JsonElement element, string propertyName, string[] defaultValue)
        {
            if (element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Array)
            {
                var result = new List<string>();
                foreach (var item in property.EnumerateArray())
                {
                    result.Add(item.GetString() ?? "");
                }
                return result.ToArray();
            }
            return defaultValue;
        }
        private string GetNestedJsonPropertyStringValue(JsonElement element, string propertyPath, string defaultValue)
        {
            var properties = propertyPath.Split('.');
            var currentElement = element;

            foreach (var prop in properties)
            {
                if (!currentElement.TryGetProperty(prop, out currentElement))
                    return defaultValue;
            }

            return currentElement.ValueKind == JsonValueKind.String ?
                currentElement.GetString() ?? defaultValue : defaultValue;
        }
    }
}
