namespace rate_it_api.Core.DTOs
{
    public class ItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
