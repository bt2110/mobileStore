using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mobileStore.ApiIntegration;
using mobileStore.Utilities.Constants;
using mobileStore.ViewModels.Sale;
using mobileStore.ViewModels.Utilities.Enums;
using mobileStore.WebApp.Models;
using Newtonsoft.Json;
using Stripe;
using System.Security.Claims;
using PaymentMethod = mobileStore.ViewModels.Utilities.Enums.PaymentMethod;


namespace mobileStore.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderApiClient;
        private readonly IUserApiClient _userApiClient;
        private readonly IDiscountCodeApiClient _discountApiClient;

        public CartController(IProductApiClient productApiClient, IOrderApiClient orderApiClient, IUserApiClient userApiClient, IDiscountCodeApiClient discountApiClient)
        {
            _productApiClient = productApiClient;
            _orderApiClient = orderApiClient;
            _userApiClient = userApiClient;
            _discountApiClient = discountApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _productApiClient.GetById(id);

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            var currentCart = new CartViewModel();
            currentCart.CartItems = new List<CartItemViewModel>();

            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            }

            int quantity = 1;

            if (currentCart.CartItems.Any(x => x.ProductId == id))
            {
                if (currentCart.CartItems.First(x => x.ProductId == id).Quantity == product.Stock)
                {
                    return Ok(currentCart.CartItems);
                }

                quantity = currentCart.CartItems.First(x => x.ProductId == id).Quantity + quantity;
                currentCart.CartItems.First(x => x.ProductId == id).Quantity = quantity;
            }
            else
            {
                var cartItem = new CartItemViewModel()
                {
                    ProductId = id,
                    Image = product.ThumbnailImage,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    DiscountPercentage = product.DiscountPercentage,
                };

                currentCart.CartItems.Add(cartItem);
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

            return Ok(currentCart);
        }

        public async Task<IActionResult> UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            var currentCart = new CartViewModel();
            currentCart.CartItems = new List<CartItemViewModel>();

            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            }

            foreach (var item in currentCart.CartItems)
            {
                if (item.ProductId == id)
                {
                    var product = await _productApiClient.GetById(item.ProductId);
                    var productStock = product.Stock;

                    if (quantity == 0 && currentCart.CartItems.Count > 1)
                    {
                        currentCart.CartItems.Remove(item);
                        break;
                    }
                    else if (quantity == 0 && currentCart.CartItems.Count == 1) // for what ?
                    {
                        currentCart.CartItems.Remove(item);
                        currentCart.Promotion = 0;
                        break;
                    }
                    else if (quantity > productStock)
                    {
                        return Content("quantity is greater than stock");
                    }
                    item.Quantity = quantity;
                }
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

            return Ok(currentCart);
        }

    [HttpGet]
        public IActionResult GetListItems()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            var currentCart = new CartViewModel();
            currentCart.CartItems = new List<CartItemViewModel>();
            

            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            }

            return Ok(currentCart);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Checkout()
        {
            return View(GetCheckoutViewModel());
        }

        private CheckoutViewModel GetCheckoutViewModel()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            //var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var claims = User.Claims.ToList();

            var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var address = claims.FirstOrDefault(x => x.Type == ClaimTypes.StreetAddress).Value;
            var phoneNumber = claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone).Value;

            var currentCart = new CartViewModel();
            currentCart.CartItems = new List<CartItemViewModel>();

            if (session != null)
            {
                //currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            }

            var checkoutVm = new CheckoutViewModel()
            {
                CartItems = currentCart.CartItems,
                CheckoutModel = new CheckoutRequest(),
                Name = name.ToString(),
                Address = address.ToString(),
                Email = email.ToString(),
                PhoneNumber = phoneNumber.ToString(),
                Promotion = currentCart.Promotion,
                DiscountCode = currentCart.DiscountCode
            };

            return checkoutVm;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            var currentCart = new CartViewModel();
            currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            long price = 0;
            float sub_price = 0;

            if (currentCart.Promotion != 0)
            {
                var promotion = currentCart.Promotion;
                sub_price = (float)(currentCart.CartItems.Sum(x => x.Price * x.Quantity));
                price = (long)((long)sub_price * (100f - promotion) / 100f);
            }
            else
            {
                price = (long)currentCart.CartItems.Sum(x => x.Price * x.Quantity);
            }

            // Tìm Guid của người mua để gán vào order
            var claims = User.Claims.ToList();
            var userId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var users = await _userApiClient.GetAll();
            var x = users.FirstOrDefault(x => x.Id.ToString() == userId);

            // Order detail là lấy từ session chứ không lấy qua CheckoutViewModel, vì model binding không có bind cái danh sách sản phẩm
            var model = GetCheckoutViewModel();
            var orderDetails = new List<OrderDetailViewModel>();

            foreach (var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailViewModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            var email1 = model.Email;
            var checkoutRequest = new CheckoutRequest()
            {
                UserID = x.Id,
                Address = request.CheckoutModel.Address,
                Name = request.CheckoutModel.Name,
                Email = email1,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                OrderDetails = orderDetails,
                PaymentMethod = PaymentMethod.COD,
                Total = price,
            };

            if (model.DiscountCode != null)
            {
                var discounts = await _discountApiClient.GetAll();
                var discount = discounts.FirstOrDefault(x => x.Code == model.DiscountCode);
                checkoutRequest.DiscountCodeId = discount.Id;
            }

            var result = await _orderApiClient.CreateOrder(checkoutRequest);

            if (result != "Failed")
            {
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
                currentCart.CartItems.Clear();
                currentCart.Promotion = 0;
                HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
                TempData["SuccessMsg"] = "Order purchased successful";
                return View(request);
            }

            ModelState.AddModelError("", "Đặt hàng thất bại");
            return View(request);
        }

        [TempData]
        public string TotalAmount { get; set; }

        [HttpGet]
        public IActionResult Payment()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            ViewBag.cart = currentCart;
            ViewBag.DollarAmount = currentCart.Sum(x => x.Price * x.Quantity);
            ViewBag.total = Math.Round(ViewBag.DollarAmount, 2) * 100;
            ViewBag.total = Convert.ToInt64(ViewBag.total);
            long total = ViewBag.total;
            TotalAmount = total.ToString();
            return View();
        }

    }
}
