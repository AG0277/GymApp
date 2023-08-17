using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Models
{
    public class Meal
    {
        public Meal()
        {
            MealProducts = new List<MealProduct>();
        }
        [Key]
        public string MealId { get; set; }
        public string MealName { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Kcal must be positive")]
        public float kcal { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Proteins must be positive")]
        public float protein { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Carbs must be positive")]
        public float carbs { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Fat must be positive")]
        public float fat { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Grams must be positive")]
        public float grams { get; set; }
        public DateTime? date { get; set; }

        [ForeignKey("UserId")]
        public  AppUser User { get; set; }
        public ICollection<MealProduct> MealProducts { get; set; }
    }
}
