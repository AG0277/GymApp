using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GymApp.Models
{
    public class Product
    {

        public Product()
        {
            MealProducts = new List<MealProduct>();
        }

        [Key] 
        public string? ProductId { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Kcal must be positive")]
        public float kcal { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Proteins must be positive")]
        public float protein { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Carbs must be positive")]
        public float carbs { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Fat must be positive")]
        public float fat { get; set; }
        [Range(0f, float.MaxValue, ErrorMessage = "Fat must be positive")]
        public float grams { get; set; }
        public ICollection<MealProduct> MealProducts { get; set; }
    }
}
