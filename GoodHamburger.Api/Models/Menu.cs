namespace Good_Hamburger.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Food> Foods { get; set; }
    }
}
