using GymApp.Models;

namespace GymApp.ViewModels
{
    public class DeleteMealViewModel
    {

        public string? MealName;
        public List<Product>? products;
        public MealSummaryViewModel? mealSummary;
        public DeleteMealViewModel()
        {
            MealName = null; 
            products = new List<Product>();
            mealSummary = new MealSummaryViewModel(); 
        }

    }
}
