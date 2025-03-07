using Microsoft.EntityFrameworkCore;
using rate_it_api.Core.Entities;

namespace rate_it_api.Infra.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        AppDbContext _context;
        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rating> GetRatingsForItemAsync(int itemId)
        {
            var item = await _context.Ratings.FindAsync(itemId);
            return item;
        }
        public async Task<Rating> SubmitRatingAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<IEnumerable<Rating>> GetUserRatingsWithItemsAsync(string userId)
        {
            return await _context.Ratings
                .Where(r => r.UserId == userId)
                .Include(r => r.Item)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
