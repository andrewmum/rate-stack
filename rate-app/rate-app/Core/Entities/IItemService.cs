using rate_it_api.Core.Entities;

namespace rate_it_api.Core.Services
{
    public interface IItemService
    {
        Task<Item> GetItemByIdAsync(int id);
        Task<List<Item>> SearchItemsAsync(string searchString);
        Task<Item> CreateItemAsync(string name, Category category = Category.Custom, string description = "");

    }
}
