namespace Good_Hamburger.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        
        public int FoodId { get; set; }
        public Food Food { get; set; } = null!;
        
        public decimal UnitPrice { get; set; }
        // De acordo com as regras, cada pedido pode conter apenas um de cada item.
        // Mas a modelagem permite quantidade para ser genérica, validaremos no Service.
        public int Quantity { get; set; } = 1;
    }
}
