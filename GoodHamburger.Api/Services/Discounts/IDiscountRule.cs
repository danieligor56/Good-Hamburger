using Good_Hamburger.Models;

namespace Good_Hamburger.Services.Discounts
{
    public interface IDiscountRule
    {
        // Define a prioridade da regra (maiores descontos primeiro)
        int Priority { get; }
        
        // Verifica se o pedido atende aos critérios da regra
        bool IsMatch(List<OrderItem> items);
        
        // Executa o cálculo e retorna o valor e a descrição
        (decimal discountAmount, string description) Calculate(decimal subtotal);
    }
}
