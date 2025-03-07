using Microsoft.AspNetCore.Mvc;
using rate_it_api.Core.Entities;
using rate_it_api.Infra.Repositories;

namespace rate_it_api.Core.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        public ItemService(IItemRepository item)
        {
            _itemRepository = item;
        }
        public async Task<Item?> GetItemByIdAsync(int id)
        {

            return await _itemRepository.GetItemByIdAsync(id);

        }
        public async Task<List<Item>> SearchItemsAsync(string searchString)
        {
            return await _itemRepository.SearchItemsAsync(searchString);
        }


        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAllItemsAsync();
        }


        public async Task<Item> CreateItemAsync(string name, Category category = Category.Custom, string description = "")
        {
            var item = new Item { Name = name, Category = category, Description = description };
            return await _itemRepository.CreateItemAsync(item);
        }

       public async Task<Item> CreateExternalItemAsync(string externalId, Category category)
        {
            return await _itemRepository.CreateExternalItemAsync(externalId, category);
        }
    }
}
