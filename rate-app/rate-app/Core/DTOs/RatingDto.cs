using rate_it_api.Core.Entities;

namespace rate_it_api.Core.DTOs
{
    public class RatingDto
    {
        public int? ItemId { get; set; }
        public string? ExternalItemId { get; set; }
        public int Score { get; set; } 
        public string? Review { get; set; }
        public Category Category { get; set; }

    }
}
