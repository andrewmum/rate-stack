namespace rate_it_api.Core.DTOs
{
    public class RatingDto
    {
        public string UserId { get; set; }
        public int ItemId { get; set; }
        public int Score { get; set; } 
        public string? Review { get; set; }
    }
}
