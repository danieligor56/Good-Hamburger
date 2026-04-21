namespace GoodHamburger.Web.Models
{
    public enum FoodType { SANDUICHE, ACOMPANHAMENTO, BEBIDA }

    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public FoodType TypeFood { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; } = new();
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public string? DiscountDescription { get; set; }
    }

    public class OrderRequestDto
    {
        public List<int> FoodIds { get; set; } = new();
    }
}
