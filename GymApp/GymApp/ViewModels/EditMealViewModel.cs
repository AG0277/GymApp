using GymApp.Models;

namespace GymApp.ViewModels
{
    public class EditMealViewModel
    {
        public Meal Meal { get; set; }
        public List< Product> products { get; set; }
        public List<MealProductViewModel> MealProductVM { get; set; }

        public void AddProduct(Product product)
        {
            Meal.kcal += product.kcal;
            Meal.protein += product.protein;
            Meal.carbs += product.carbs;
            Meal.fat += product.fat;
            Meal.grams += product.grams;
           
        }
        public void SubtractProduct(Product product)
        {
            Meal.kcal -= product.kcal;
            Meal.protein -= product.protein;
            Meal.carbs -= product.carbs;
            Meal.fat -= product.fat;
            Meal.grams -= product.grams;
        }
    }
}
