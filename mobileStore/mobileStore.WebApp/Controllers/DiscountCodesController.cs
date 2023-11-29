using Microsoft.AspNetCore.Mvc;
using mobileStore.ApiIntegration;
using mobileStore.Utilities.Constants;
using mobileStore.ViewModels.Catalog.Products;
using mobileStore.WebApp.Models;
using Newtonsoft.Json;

namespace mobileStore.WebApp.Controllers
{
    public class DiscountCodesController : Controller
    {
        private readonly IDiscountCodeApiClient _discountApiClient;

        public DiscountCodesController(
            IDiscountCodeApiClient discountClient)
        {
            _discountApiClient = discountClient;
        }

        public async Task<IActionResult> Index()
        {
            var request = new GetManageProductPagingRequest()
            {
                PageIndex = 1,
                PageSize = 4,
            };
            var coupons = await _discountApiClient.GetAllPaging(request);
            return View(coupons);
        }

        public async Task<IActionResult> Detail(int discountId)
        {
            var discount = await _discountApiClient.GetById(discountId);
            return View(discount);
        }

        [HttpPost]
        public async Task<int> ApplyDiscount(string code)
        {
            var discounts = await _discountApiClient.GetAll();
            var discount = discounts.FirstOrDefault(x => x.Code == code);

            if (discount == null)
            {
                return -2;

            }
            else if (discount.Count <= 0)
            {
                return -1;
            }

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            var currentCart = new CartViewModel();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            }
            if (currentCart.Promotion != 0)
            {
                return 0;
            }
            currentCart.Promotion = discount.Promotion;
            currentCart.DiscountCode = discount.Code;
            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

            return currentCart.Promotion;
        }
    }
}
