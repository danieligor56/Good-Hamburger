using Good_Hamburger.Models;
using Good_Hamburger.Enums;

namespace Good_Hamburger.Services.Discounts
{
    public class ComboCompletoRule : IDiscountRule
    {
        public int Priority => 3; // Maior prioridade por ser o maior desconto

        public bool IsMatch(List<OrderItem> items)
        {
            var types = items.Select(i => i.Food.TypeFood).ToList();
            return types.Contains(FoodType.SANDUICHE) && 
                   types.Contains(FoodType.ACOMPANHAMENTO) && 
                   types.Contains(FoodType.BEBIDA);
        }

        public (decimal discountAmount, string description) Calculate(decimal subtotal)
        {
            return (subtotal * 0.20m, "Combo Completo: 20% de desconto");
        }
    }
}
