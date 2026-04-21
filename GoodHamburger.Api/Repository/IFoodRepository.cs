using Good_Hamburger.Models;

namespace Good_Hamburger.Repository
{
    public interface IFoodRepository
    {
        Task<IEnumerable<Food>> GetAllAsync();
        Task<Food?> GetByIdAsync(int id);
    }
}
