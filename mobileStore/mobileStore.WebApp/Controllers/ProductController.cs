using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mobileStore.ApiIntegration;
using mobileStore.ViewModels.Catalog.Products;
using mobileStore.WebApp.Models;
using System.Security.Claims;

namespace mobileStore.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IUserApiClient _userApiClient;


        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IUserApiClient userApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productApiClient.GetById(id);
            ViewBag.ProductId = id;

            var reviews = product.Reviews;
            ViewBag.Comments = reviews;

            var ratings = product.Reviews;
            if (ratings != null)
            {
                var ratingSum = ratings.Sum(d => d.Rating);
                ViewBag.RatingSum = ratingSum;
                var ratingCount = ratings.Count();
                ViewBag.RatingCount = ratingCount;
            }
            else
            {
                ViewBag.RatingSum = 0;
                ViewBag.RatingCount = 0;
            }

            var productDetailViewModel = new ProductDetailViewModel()
            {
                Product = product,
                ListOfReviews = reviews
            };

            // get user review name
            if(reviews != null)
            {
                foreach (var review in productDetailViewModel.ListOfReviews)
                {
                    Guid userId = new Guid(review.UserId.ToString());
                    var user = await _userApiClient.GetById(userId);
                    review.UserName = user.ResultObj.FirstName;
                }
            }

            return View(productDetailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddReview(ProductDetailViewModel model)
        {
            var productDetailVM = new ProductDetailViewModel()
            {
                ProductId = model.ProductId,
                Review = model.Review,
                Rating = model.Rating,
                UserCommentId = model.UserCommentId
            };

            var request = await _productApiClient.AddReview(productDetailVM);

            int Id = int.Parse(request);

            return RedirectToAction("Detail", "Product", new { id = Id });
        }

        public async Task<IActionResult> Category(int? id, int page = 1)
        {
            var products = await _productApiClient.GetPagings(new GetManageProductPagingRequest()
            {
                CategoryId = id,
                PageIndex = page,
                PageSize = 9
            });
                var category = id.HasValue ? await _categoryApiClient.GetById(id.Value) : null;

            return View(new ProductCategoryViewModel()
            {
                Category = category,
                Products = products
            });
        }
    }
}
