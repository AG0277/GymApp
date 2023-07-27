using GymApp.Data;
using GymApp.Models;
using System.Linq;

namespace Services
{
    public class Services
    {
        private readonly ApplicationDbContext _dbContext;

        public Services(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public IQueryable<Product> GetProductsForMeal(int mealId)
        //{
        //    var products = _dbContext.Products
        //        .Where(p => p.MealProducts.Any(mp => mp.MealId == mealId));

        //    return products;
        //}
    }
}