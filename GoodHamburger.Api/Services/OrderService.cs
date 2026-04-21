using Good_Hamburger.Models;
using Good_Hamburger.Enums;
using Good_Hamburger.Repository;
using Good_Hamburger.Services.Discounts;

namespace Good_Hamburger.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(List<int> foodIds);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task DeleteOrderAsync(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IEnumerable<IDiscountRule> _discountRules;

        public OrderService(
            IOrderRepository orderRepository, 
            IFoodRepository foodRepository,
            IEnumerable<IDiscountRule> discountRules)
        {
            _orderRepository = orderRepository;
            _foodRepository = foodRepository;
            _discountRules = discountRules;
        }

        public async Task<Order> CreateOrderAsync(List<int> foodIds)
        {
            var foods = new List<Food>();
            foreach (var id in foodIds)
            {
                var food = await _foodRepository.GetByIdAsync(id);
                if (food == null) throw new Exception($"Produto com ID {id} não encontrado.");
                foods.Add(food);
            }

            // Validação usando LINQ: Apenas um de cada tipo
            var duplicates = foods.GroupBy(f => f.TypeFood)
                                  .Where(g => g.Count() > 1)
                                  .Select(g => g.Key)
                                  .ToList();

            if (duplicates.Any())
            {
                var types = string.Join(", ", duplicates);
                throw new Exception($"O pedido contém itens duplicados para os tipos: {types}. Cada pedido permite apenas um de cada.");
            }

            var order = new Order
            {
                Items = foods.Select(f => new OrderItem
                {
                    FoodId = f.Id,
                    Food = f,
                    UnitPrice = f.Price,
                    Quantity = 1
                }).ToList(),
                Subtotal = foods.Sum(f => f.Price)
            };

            // Cálculo de Desconto usando MOTOR DE REGRAS e LINQ
            ApplyBestDiscount(order);

            return await _orderRepository.CreateAsync(order);
        }

        private void ApplyBestDiscount(Order order)
        {
            // Busca a regra com maior prioridade que se encaixe no pedido
            var bestRule = _discountRules
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault(r => r.IsMatch(order.Items));

            if (bestRule != null)
            {
                var (amount, description) = bestRule.Calculate(order.Subtotal);
                order.DiscountAmount = amount;
                order.DiscountDescription = description;
            }
            else
            {
                order.DiscountAmount = 0;
                order.DiscountDescription = null;
            }

            order.Total = order.Subtotal - order.DiscountAmount;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }
    }
}
