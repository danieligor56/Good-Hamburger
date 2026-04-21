using Microsoft.AspNetCore.Mvc;
using Good_Hamburger.Repository;

namespace Good_Hamburger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IFoodRepository _foodRepository;

        public MenuController(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMenu()
        {
            var menu = await _foodRepository.GetAllAsync();
            return Ok(menu);
        }
    }
}
