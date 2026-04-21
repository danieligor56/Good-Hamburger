using Microsoft.EntityFrameworkCore;
using Good_Hamburger.Models;

namespace Good_Hamburger.Repository
{
    public class FoodRepository : IFoodRepository
    {
        private readonly AppDbContext _context;

        public FoodRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Food>> GetAllAsync()
        {
            return await _context.Foods.ToListAsync();
        }

        public async Task<Food?> GetByIdAsync(int id)
        {
            return await _context.Foods.FindAsync(id);
        }
    }
}
