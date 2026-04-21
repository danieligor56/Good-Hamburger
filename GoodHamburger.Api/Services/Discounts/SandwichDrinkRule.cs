using Good_Hamburger.Models;
using Good_Hamburger.Enums;

namespace Good_Hamburger.Services.Discounts
{
    public class SandwichDrinkRule : IDiscountRule
    {
        public int Priority => 2;

        public bool IsMatch(List<OrderItem> items)
        {
            var types = items.Select(i => i.Food.TypeFood).ToList();
            return types.Contains(FoodType.SANDUICHE) && types.Contains(FoodType.BEBIDA);
        }

        public (decimal discountAmount, string description) Calculate(decimal subtotal)
        {
            return (subtotal * 0.15m, "Sanduíche + Bebida: 15% de desconto");
        }
    }
}
