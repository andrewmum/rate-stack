using rate_it_api.Core.DTOs;
using rate_it_api.Core.Entities;

namespace rate_app.Core.DTOs
{
    public class UserRatingWithItemDto
    {
        public int RatingId { get; set; }
        public int Score { get; set; }
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ItemId { get; set; }
        public string ExternalID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string? Description { get; set; }
        public ItemDto Item { get; set; }
    }
    
}
