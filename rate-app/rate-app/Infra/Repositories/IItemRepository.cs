using rate_it_api.Core.Entities;

namespace rate_it_api.Infra.Repositories
{
    public interface IItemRepository
    {
        Task<Item?> GetItemByIdAsync(int id);
        Task<List<Item>> SearchItemsAsync(string searchString);
        Task<List<Item>> GetAllItemsAsync();
        Task<Item> CreateItemAsync(Item item);
        Task<Item> CreateExternalItemAsync(string externalId, Category category);
    }
}
