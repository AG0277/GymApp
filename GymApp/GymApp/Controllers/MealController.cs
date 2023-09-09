using GymApp.Data;
using GymApp.Models;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;

namespace GymApp.Controllers
{
    public class MealController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private List<MealProductViewModel> MealProductsVM;
        private MealSummaryViewModel _MealSummaryVM;

        public MealController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
            _userManager = userManager;
            MealProductsVM= new List<MealProductViewModel>();
            _MealSummaryVM = new MealSummaryViewModel();


            var serializedMealProduct = _httpContextAccessor.HttpContext.Session.GetString("MealProducts");
            if (!string.IsNullOrEmpty(serializedMealProduct))
            {
                MealProductsVM = JsonConvert.DeserializeObject<List<MealProductViewModel>>(serializedMealProduct);
            }

            var serializedMealSummary = _httpContextAccessor.HttpContext.Session.GetString("MealSummary");
            if (!string.IsNullOrEmpty(serializedMealSummary))
            {
                _MealSummaryVM = JsonConvert.DeserializeObject<MealSummaryViewModel>(serializedMealSummary);
            }
        }

        public IActionResult Index()
        {
            var currentUser = _httpContextAccessor.HttpContext.User.GetUserID();
            var userMeals = _db.Meals.Where(r => r.User.Id == currentUser.ToString()).ToList();
            var mealViewModel = new MealViewModel()
            {
                Meals = userMeals
            };
            return View(mealViewModel);
        }

        public IActionResult AddProductsToMeal(string productId)
        {
            var productAttributes = _db.Products.FirstOrDefault(p => p.ProductId == productId);

            var mealProduct = new MealProductViewModel
            {
                ProductId = productId,
                ProductGrams = productAttributes.grams
            };

            MealProductsVM.Add(mealProduct);
            var serializedMealProduct = JsonConvert.SerializeObject(MealProductsVM);
            _httpContextAccessor.HttpContext.Session.SetString("MealProducts", serializedMealProduct);


            _MealSummaryVM.AddProduct(productAttributes);
            var serializedMealSummary = JsonConvert.SerializeObject(_MealSummaryVM);
            _httpContextAccessor.HttpContext.Session.SetString("MealSummary", serializedMealSummary);


            return Json(productAttributes);
        }

        public IActionResult GetSelectedProducts()
        {
            var listOfSessionProducts = new List<Product>();
            foreach(var obj in MealProductsVM) 
            {
                var product = _db.Products.FirstOrDefault(p => p.ProductId == obj.ProductId);
                listOfSessionProducts.Add(product);
            }
            return Json(listOfSessionProducts);
        }
        public IActionResult UpdateSummary()
        {
            return Json(_MealSummaryVM);
        }


        public IActionResult SearchBar(string query)
        {
            var searchResults = _db.Products
                .Where(r => r.ProductName.Contains(query))
                .Select(r => new { r.ProductId, r.ProductName })
                .ToList();

            return Json(searchResults);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Meal obj)
        {
           
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                obj.User = user;
                ModelState.Remove("User");
                ModelState.Remove("MealId");
            }
            obj.date = DateTime.Now;
            obj.protein = _MealSummaryVM.TotalProtein;
            obj.kcal = _MealSummaryVM.TotalKcal;
            obj.fat = _MealSummaryVM.TotalFat;
            obj.carbs = _MealSummaryVM.TotalCarbs;
            obj.grams = _MealSummaryVM.TotalGrams;

            if (ModelState.IsValid)
            {
                _db.Meals.Add(obj);
                _db.SaveChanges();

                foreach(var objMealProductsVM in MealProductsVM)
                {
                    var MealProduct = new MealProduct();
                    MealProduct.ProductId = objMealProductsVM.ProductId;
                    MealProduct.ProductGrams = objMealProductsVM.ProductGrams;
                    MealProduct.MealId = obj.MealId;
                    _db.MealProducts.Add(MealProduct);
                }
                _db.SaveChanges();
                return RedirectToAction("Index", "Meal");
            }
            else
            {
                foreach (var err in ModelState.Keys)
                {
                    var error = ModelState[err].Errors;
                    foreach(var errormessage in error)
                    {
                        string er = errormessage.ErrorMessage;
                    }
                }
            }

            return View();
        }
    }
}
