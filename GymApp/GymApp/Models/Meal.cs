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
        public float kcal { get; set; }
        public float protein { get; set; }
        public float carbs { get; set; }
        public float fat { get; set; }
        public float grams { get; set; }
        public DateTime date { get; set; }

        [ForeignKey("UserId")]
        public  AppUser User { get; set; }
        public ICollection<MealProduct> MealProducts { get; set; }
    }
}
