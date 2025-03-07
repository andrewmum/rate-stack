using rate_it_api.Core.Entities;

namespace rate_it_api.Core.DTOs
{
    public abstract class ItemDto
    {
        public string Id { get; set; }
        public Category Category { get; set; }
    }

    public class BookDetailsDto : ItemDto
    {
        public BookDetailsDto()
        {
            Category = Category.Book;
        }
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string PublishedDate { get; set; }
        public string[] Categories { get; set; }

    }

    public class PlaceDetailsDto : ItemDto
    {
        public PlaceDetailsDto()
        {
            Category = Category.Resturaunt;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Address { get; set; }
    }
}
