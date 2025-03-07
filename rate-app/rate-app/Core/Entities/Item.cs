namespace rate_it_api.Core.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string ExternalID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public Category Category { get; set; } = Category.Custom; 
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Rating> Ratings { get; set; } = new();
    }


    public enum Category
    {
        Resturaunt,
        Experience,
        Recipe,
        Custom,
        Book,
    }
}
