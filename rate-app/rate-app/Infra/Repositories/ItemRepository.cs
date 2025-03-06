using Microsoft.EntityFrameworkCore;
using rate_it_api.Core.Entities;

namespace rate_it_api.Infra.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _context.Items.FindAsync(id);

        }
        public async Task<List<Item>> SearchItemsAsync(string searchString) 
        {
            return await _context.Items.Where(item => item.Name.Contains(searchString)).ToListAsync();
        }
        public async Task<Item> CreateItemAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
