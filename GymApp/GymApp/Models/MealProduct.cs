namespace GymApp.Models
{
    public class MealProduct
    {
        public string MealId { get; set; }
        public string ProductId { get; set; }
        public float ProductGrams { get; set; }
        public  Meal Meal { get; set; }
        public  Product Product { get; set; }
    }
}
