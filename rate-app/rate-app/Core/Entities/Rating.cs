namespace rate_it_api.Core.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; } = null!;

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public int Score { get; set; } 
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
