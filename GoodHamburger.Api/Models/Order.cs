using System.ComponentModel.DataAnnotations;

namespace Good_Hamburger.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        
        // Propriedade para armazenar o resumo do desconto aplicado (ex: "20% combo completo")
        public string? DiscountDescription { get; set; }
    }
}
