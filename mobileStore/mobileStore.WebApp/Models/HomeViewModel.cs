using mobileStore.Data.Entities;
using mobileStore.ViewModels.Catalog.Products;

namespace mobileStore.WebApp.Models
{
    public class HomeViewModel
    {
        public List<ProductViewModel> NewProducts { get; set; }
        public List<ProductViewModel> FeaturedProducts { get; set; }
        public List<ProductViewModel> DiscountedProducts { get; set; }

    }
}
