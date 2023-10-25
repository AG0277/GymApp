using GymApp.Data;
using GymApp.Models;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nancy;
using Nancy.Diagnostics;
using Nancy.Json;
using Nancy.Routing.Trie;
using Nancy.Session;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.Json.Nodes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GymApp.Controllers
{
    public class MealController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private List<MealProductViewModel> MealProductsVM;
        private MealSummaryViewModel _MealSummaryVM;
        private EditMealViewModel _EditMealVM;

        public MealController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
            _userManager = userManager;
            MealProductsVM= new List<MealProductViewModel>();
            _MealSummaryVM = new MealSummaryViewModel();
            _EditMealVM = new EditMealViewModel();

            var serializedEditMeal = _httpContextAccessor.HttpContext.Session.GetString("EditMeal");
            if (!string.IsNullOrEmpty(serializedEditMeal))
            {
                _EditMealVM = JsonConvert.DeserializeObject<EditMealViewModel>(serializedEditMeal);
            }

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
                Product productdb = _db.Products.FirstOrDefault(p => p.ProductId == obj.ProductId);
                if (productdb != null)
                {
                    Product product = new Product
                    {
                        ProductId = productdb.ProductId,
                        ProductName = productdb.ProductName,
                        kcal = productdb.kcal,
                        protein = productdb.protein,
                        carbs = productdb.carbs,
                        fat = productdb.fat,
                        grams = productdb.fat,
                        MealProducts = productdb.MealProducts,
                    };
                    var productRatio = obj.ProductGrams / product.grams;
                    product.kcal *= productRatio;
                    product.protein *= productRatio;
                    product.carbs *= productRatio;
                    product.fat *= productRatio;
                    product.grams *= productRatio;
                    listOfSessionProducts.Add(product);
                }
            }
            return Json(listOfSessionProducts);
        }
        public IActionResult UpdateSummary()
        {
            return Json(_MealSummaryVM);
        }

        public IActionResult UpdateEditSummary()
        {
            return Json(_EditMealVM);
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

                Dictionary<string, float> checkForDuplicates = new Dictionary<string, float>();
                List<MealProductViewModel> itemsToRemove = new List<MealProductViewModel>();
                foreach (var objMealProductsVM in MealProductsVM)
                {
                    if (!checkForDuplicates.ContainsKey(objMealProductsVM.ProductId))
                        checkForDuplicates.Add(objMealProductsVM.ProductId, objMealProductsVM.ProductGrams);
                    else
                    {
                        itemsToRemove.Add(objMealProductsVM);
                        for (var i = 0; i < MealProductsVM.Count(); i++)
                            if (MealProductsVM.ElementAt(i).ProductId == objMealProductsVM.ProductId)
                            {
                                MealProductsVM.ElementAt(i).ProductGrams += objMealProductsVM.ProductGrams;
                            }
                    }
                }

                foreach (var itemToRemove in itemsToRemove)
                {
                    MealProductsVM.Remove(itemToRemove);
                }

                foreach (var objMealProductsVM in MealProductsVM)
                {
                    var MealProduct = new MealProduct();
                    MealProduct.ProductId = objMealProductsVM.ProductId;
                    MealProduct.ProductGrams = objMealProductsVM.ProductGrams;
                    MealProduct.MealId = obj.MealId;
                    _db.MealProducts.Add(MealProduct);
                }
                _db.SaveChanges();
                _httpContextAccessor.HttpContext.Session.Remove("MealSummary");
                _httpContextAccessor.HttpContext.Session.Remove("MealProducts");

                return RedirectToAction("Index", "Meal");
            }
            return View();
        }
        public async Task<IActionResult> Delete(string MealId)
        {

            DeleteMealViewModel DeleteMealVM = new DeleteMealViewModel();


            Meal? mealdb = new Meal();
            mealdb = await _db.Meals.FindAsync(MealId);
            if (mealdb != null)
            {
                DeleteMealVM.mealSummary.AddMeal(mealdb);
                DeleteMealVM.MealName=mealdb.MealName;
            }
            else
                return NotFound();


            List<MealProduct> matchingMealProducts = await _db.MealProducts
                .Where(mp => mp.MealId == MealId)
                .ToListAsync();

            for (var i = 0; i < matchingMealProducts.Count; i++)
            {
                var product = await _db.Products.FindAsync(matchingMealProducts.ElementAt(i).ProductId);
                var productRatio = matchingMealProducts.ElementAt(i).ProductGrams / product.grams;
                product.kcal *= productRatio;
                product.protein *= productRatio;
                product.carbs *= productRatio;
                product.fat *= productRatio;
                product.grams *= productRatio;
                DeleteMealVM.products.Add(product);
            }

                return View(DeleteMealVM);
        }
        [HttpPost]
        public  IActionResult DeleteProduct(string jsonData)
        {
            try
            {
                JObject json = JObject.Parse(jsonData);
                Product product = json.ToObject<Product>();
                MealProductViewModel mp = new MealProductViewModel();
                for (int i = 0; i < MealProductsVM.Count(); i++)
                {
                    if (MealProductsVM[i].ProductId == product.ProductId)
                        mp = MealProductsVM.ElementAt(i);
                }

                MealProductsVM.Remove(mp);
                _MealSummaryVM.SubtractProduct(product);

                var serializedMealSummary = JsonConvert.SerializeObject(_MealSummaryVM);
                _httpContextAccessor.HttpContext.Session.SetString("MealSummary", serializedMealSummary);

                var serializedMealProducts = JsonConvert.SerializeObject(MealProductsVM);
                _httpContextAccessor.HttpContext.Session.SetString("MealProducts", serializedMealProducts);

                return Json(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to delete product");
                return Json(false);
            }
        }
        [HttpPost]
        public  IActionResult Delete(Meal meal)
        {
            _db.Meals.Remove(meal);
            _db.SaveChanges();
            return RedirectToAction("Index", "Meal");
        }
        [HttpPost]
        public IActionResult UpdateEditProduct([FromBody] dynamic data)
        {
            string jsonData = data.ToString();

            JObject jsonObject = JObject.Parse(jsonData);

            Product product = jsonObject["productAttributes"].ToObject<Product>();
            Product newProduct = jsonObject["productAttributes"].ToObject<Product>();
            var newGrams = jsonObject["newValue"].ToObject<float>();
            var oldGrams = jsonObject["oldValue"].ToObject<float>();

            MealProductViewModel mp = new MealProductViewModel();
            for (int i = 0; i < MealProductsVM.Count(); i++)
            {
                if (MealProductsVM[i].ProductId == product.ProductId && MealProductsVM[i].ProductGrams == oldGrams)
                {
                    MealProductsVM.ElementAt(i).ProductGrams = newGrams;
                    break;
                }
            }
            var oldRatio = product.grams / oldGrams;
            var newRatio = product.grams / newGrams;
            product.kcal /= oldRatio;
            product.protein /= oldRatio;
            product.carbs /= oldRatio;
            product.fat /= oldRatio;
            product.grams /= oldRatio;
            _MealSummaryVM.SubtractProduct(product);
            newProduct.kcal /= newRatio;
            newProduct.protein /= newRatio;
            newProduct.carbs /= newRatio;
            newProduct.fat /= newRatio;
            newProduct.grams = newGrams;
            _MealSummaryVM.AddProduct(newProduct);

            var serializedMealSummary = JsonConvert.SerializeObject(_MealSummaryVM);
            _httpContextAccessor.HttpContext.Session.SetString("MealSummary", serializedMealSummary);

            var serializedMealProducts = JsonConvert.SerializeObject(MealProductsVM);
            _httpContextAccessor.HttpContext.Session.SetString("MealProducts", serializedMealProducts);

            return Json(newProduct);
        }
        public async Task<IActionResult> Edit(string mealid)
        {
            if (_EditMealVM.Meal == null)
            {
                Meal meal = await _db.Meals.FindAsync(mealid);
                _EditMealVM.Meal = meal;
                List<MealProduct> mealProducts = new List<MealProduct>();
                mealProducts = await _db.MealProducts
                    .Where(mp => mp.MealId == mealid)
                    .ToListAsync();
                List<Product> products = new List<Product>();
                List<MealProductViewModel> ListMealProductVM = new List<MealProductViewModel>(); ;
                foreach (var obj in mealProducts)
                {
                    Product product = await _db.Products.FindAsync(obj.ProductId);
                    var gramsRatio = product.grams / obj.ProductGrams;
                    product.kcal /= gramsRatio;
                    product.protein /= gramsRatio;
                    product.carbs /= gramsRatio;
                    product.fat /= gramsRatio;
                    product.grams /= gramsRatio;
                    product.MealProducts = null;
                    products.Add(product);
                    MealProductViewModel MealProductVM = new MealProductViewModel();
                    MealProductVM.ProductId = product.ProductId;
                    MealProductVM.ProductGrams = product.grams;
                    ListMealProductVM.Add(MealProductVM);
                }
                _EditMealVM.MealProductVM = ListMealProductVM;
                _EditMealVM.products = products;
                var serializedEditMeal = JsonConvert.SerializeObject(_EditMealVM);
                _httpContextAccessor.HttpContext.Session.SetString("EditMeal", serializedEditMeal);
                return View(_EditMealVM);
            }
            else 
             return View(_EditMealVM); 
        }
        public IActionResult AddProductToEditViewModel(string productId)
        {
            Product product = _db.Products.FirstOrDefault(p => p.ProductId == productId);

            _EditMealVM.AddProduct(product);
            _EditMealVM.products.Add(product);
            MealProductViewModel MealProductVM = new MealProductViewModel();
            MealProductVM.ProductId = product.ProductId;
            MealProductVM.ProductGrams = product.grams;
            _EditMealVM.MealProductVM.Add(MealProductVM);
            var serializedEditMeal = JsonConvert.SerializeObject(_EditMealVM);
            _httpContextAccessor.HttpContext.Session.SetString("EditMeal", serializedEditMeal);

            return Json(product);
        }

        public IActionResult DeleteProductAtEditViewModel(string jsonData)
        {
            Product product = JsonConvert.DeserializeObject<Product>(jsonData);
            _EditMealVM.SubtractProduct(product);
            _EditMealVM.products.Remove(product);
            MealProductViewModel MealProductVM = new MealProductViewModel();
            MealProductVM.ProductId = product.ProductId;
            MealProductVM.ProductGrams = product.grams;
            _EditMealVM.MealProductVM.Remove(MealProductVM);
            var serializedEditMeal = JsonConvert.SerializeObject(_EditMealVM);
            _httpContextAccessor.HttpContext.Session.SetString("EditMeal", serializedEditMeal);

            return Json(true);
        }

        public IActionResult DeleteProductAtEditViewModelfromView(string productId)
        {
            Product product = _db.Products.FirstOrDefault(p => p.ProductId == productId);

            _EditMealVM.SubtractProduct(product);
            foreach (var obj in _EditMealVM.products)
            {
                if (obj.ProductId == productId)
                {
                    _EditMealVM.products.Remove(obj);
                    break;
                }
            }
            foreach (var obj in _EditMealVM.MealProductVM)
            {
                if (obj.ProductId == productId)
                {
                    _EditMealVM.MealProductVM.Remove(obj);
                    break;
                }
            }
            var serializedEditMeal = JsonConvert.SerializeObject(_EditMealVM);
            _httpContextAccessor.HttpContext.Session.SetString("EditMeal", serializedEditMeal);
            return RedirectToAction("Edit", new { mealid = _EditMealVM.Meal.MealId }) ;
        }
        [HttpPost]
        public IActionResult UpdateEditEditProduct([FromBody] dynamic data)
        {
            string jsonData = data.ToString();

            JObject jsonObject = JObject.Parse(jsonData);

            Product product = jsonObject["productAttributes"].ToObject<Product>();
            Product newProduct = jsonObject["productAttributes"].ToObject<Product>();
            var newGrams = jsonObject["newValue"].ToObject<float>();
            var oldGrams = jsonObject["oldValue"].ToObject<float>();

            EditMealViewModel mp = new EditMealViewModel();
            for (int i = 0; i < _EditMealVM.MealProductVM.Count(); i++)
            {
                if (_EditMealVM.MealProductVM[i].ProductId == product.ProductId && _EditMealVM.MealProductVM[i].ProductGrams == oldGrams)
                {
                    _EditMealVM.MealProductVM.ElementAt(i).ProductGrams = newGrams;
                    break;
                }
            }
            var oldRatio = product.grams / oldGrams;
            var newRatio = product.grams / newGrams;
            product.kcal /= oldRatio;
            product.protein /= oldRatio;
            product.carbs /= oldRatio;
            product.fat /= oldRatio;
            product.grams /= oldRatio;
            _EditMealVM.SubtractProduct(product);
            newProduct.kcal /= newRatio;
            newProduct.protein /= newRatio;
            newProduct.carbs /= newRatio;
            newProduct.fat /= newRatio;
            newProduct.grams = newGrams;
            _EditMealVM.AddProduct(newProduct);

            var serializedEditMeal = JsonConvert.SerializeObject(_EditMealVM);
            _httpContextAccessor.HttpContext.Session.SetString("EditMeal", serializedEditMeal);

            return Json(newProduct);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Meal obj)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                obj.User = user;
                ModelState.Remove("User");
                ModelState.Remove("MealId");
                ModelState.Remove("MealName");
            }
            obj.date = DateTime.Now;
            obj.MealName = _EditMealVM.Meal.MealName;
            obj.protein = _EditMealVM.Meal.protein;
            obj.kcal = _EditMealVM.Meal.kcal;
            obj.fat = _EditMealVM.Meal.fat;
            obj.carbs = _EditMealVM.Meal.carbs;
            obj.grams = _EditMealVM.Meal.grams;

            if (ModelState.IsValid)
            {
                _db.Meals.Update(obj);
                _db.SaveChanges();

                Dictionary<string, float> checkForDuplicates = new Dictionary<string, float>();
                List<MealProductViewModel> itemsToRemove = new List<MealProductViewModel>();
                foreach (var objMealProductsVM in _EditMealVM.MealProductVM)
                {
                    if (!checkForDuplicates.ContainsKey(objMealProductsVM.ProductId))
                        checkForDuplicates.Add(objMealProductsVM.ProductId, objMealProductsVM.ProductGrams);
                    else
                    {
                        itemsToRemove.Add(objMealProductsVM);
                        for (var i = 0; i < _EditMealVM.MealProductVM.Count(); i++)
                            if (_EditMealVM.MealProductVM.ElementAt(i).ProductId == objMealProductsVM.ProductId)
                            {
                                _EditMealVM.MealProductVM.ElementAt(i).ProductGrams += objMealProductsVM.ProductGrams;
                            }
                    }
                }

                foreach (var itemToRemove in itemsToRemove)
                {
                    _EditMealVM.MealProductVM.Remove(itemToRemove);
                }
                List<MealProduct> dbMealProduct = new List<MealProduct>();
                dbMealProduct = _db.MealProducts.Where(mp => mp.MealId == _EditMealVM.Meal.MealId).ToList();
                Dictionary<string, float> map = new Dictionary<string, float>();
                foreach (var dbmp in dbMealProduct)
                {
                    map.Add(dbmp.ProductId, dbmp.ProductGrams);
                }
                foreach (var objMealProductsVM in _EditMealVM.MealProductVM)
                {
                    if (map.ContainsKey(objMealProductsVM.ProductId))
                    {
                        if (map[objMealProductsVM.ProductId] == objMealProductsVM.ProductGrams)
                        {
                            continue;
                        }
                        else
                        {
                            var existingMP = dbMealProduct.FirstOrDefault(mp => mp.ProductId == objMealProductsVM.ProductId);
                            existingMP.ProductGrams = objMealProductsVM.ProductGrams;
                            map[objMealProductsVM.ProductId] = objMealProductsVM.ProductGrams;
                        }
                    }
                    else
                    {
                        var MealProduct = new MealProduct();
                        MealProduct.ProductId = objMealProductsVM.ProductId;
                        MealProduct.ProductGrams = objMealProductsVM.ProductGrams;
                        MealProduct.MealId = _EditMealVM.Meal.MealId;
                        _db.MealProducts.Add(MealProduct);
                        map.Add(objMealProductsVM.ProductId, objMealProductsVM.ProductGrams);
                    }
                }
                _db.SaveChanges();
                _httpContextAccessor.HttpContext.Session.Remove("EditMeal");

                return RedirectToAction("Index", "Meal");
            }
            else
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];

                    foreach (var error in modelStateVal.Errors)
                    {
                        // Access the error message
                        var errorMessage = error.ErrorMessage;

                        // Access the exception (if any)
                        var exception = error.Exception;

                        // You can handle the error or display it as needed
                        // For example, you can add the error message to a list or display it in your view.
                    }
                }
            }
                return View();
        }
    }

}
