using Microsoft.AspNetCore.Mvc;
using mobileStore.BackendApi.Models;
using System.Diagnostics;

namespace mobileStore.BackendApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Ok();
        }
    }
}