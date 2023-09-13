using GymApp.Models;
using System.ComponentModel.DataAnnotations;

namespace GymApp.ViewModels
{
    public class MealSummaryViewModel
    {
        public MealSummaryViewModel()
        {
            TotalKcal = 0f;
            TotalProtein = 0f;
            TotalCarbs = 0f;
            TotalFat = 0f;
            TotalGrams = 0f;
        }
        [Range(0f, float.MaxValue, ErrorMessage = "Kcal must be positive")]
        public float TotalKcal { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Proteins must be positive")]
        public float TotalProtein { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Carbs must be positive")]
        public float TotalCarbs { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Fat must be positive")]
        public float TotalFat { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Grams must be positive")]
        public float TotalGrams { get; set; }

        public void AddProduct(Product product)
        {
            TotalKcal += product.kcal;
            TotalProtein += product.protein;
            TotalCarbs += product.carbs;
            TotalFat += product.fat;
            TotalGrams += product.grams;
        }
        public void SubtractProduct(Product product)
        {
            TotalKcal -= product.kcal;
            TotalProtein -= product.protein;
            TotalCarbs -= product.carbs;
            TotalFat -= product.fat;
            TotalGrams -= product.grams;
        }

        public void AddMeal(Meal meal)
        {
            TotalKcal += meal.kcal;
            TotalProtein += meal.protein;
            TotalCarbs += meal.carbs;
            TotalFat += meal.fat;
            TotalGrams += meal.grams;
        }
    }
}
