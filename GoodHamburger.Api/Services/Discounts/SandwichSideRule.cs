using Good_Hamburger.Models;
using Good_Hamburger.Enums;

namespace Good_Hamburger.Services.Discounts
{
    public class SandwichSideRule : IDiscountRule
    {
        public int Priority => 1;

        public bool IsMatch(List<OrderItem> items)
        {
            var types = items.Select(i => i.Food.TypeFood).ToList();
            return types.Contains(FoodType.SANDUICHE) && types.Contains(FoodType.ACOMPANHAMENTO);
        }

        public (decimal discountAmount, string description) Calculate(decimal subtotal)
        {
            return (subtotal * 0.10m, "Sanduíche + Acompanhamento: 10% de desconto");
        }
    }
}
