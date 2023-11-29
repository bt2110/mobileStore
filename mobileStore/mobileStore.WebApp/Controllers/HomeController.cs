using Microsoft.AspNetCore.Mvc;
using mobileStore.WebApp.Models;
using System.Diagnostics;
using mobileStore.ApiIntegration;
using Microsoft.AspNetCore.Localization;
using mobileStore.Utilities.Constants;
using System.Globalization;

namespace mobileStore.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductApiClient _productApiClient;

        public HomeController(ILogger<HomeController> logger,
            IProductApiClient productApiClient)
        {
            _logger = logger;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                NewProducts = await _productApiClient.GetNewProducts(SystemConstants.ProductSettings.NumberOfNewProducts),
                FeaturedProducts = await _productApiClient.GetFeaturedProducts(SystemConstants.ProductSettings.NumberOfNewProducts),
                DiscountedProducts = await _productApiClient.GetDiscountedProducts(SystemConstants.ProductSettings.NumberOfNewProducts),

            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contac()
        {
            return View();
        }
        public IActionResult Information()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}