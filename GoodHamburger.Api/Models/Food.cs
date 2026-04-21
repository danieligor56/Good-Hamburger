using Good_Hamburger.Enums;

namespace Good_Hamburger.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public FoodType TypeFood { get; set; }
    }
}
