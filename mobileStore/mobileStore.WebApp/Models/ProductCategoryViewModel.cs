using mobileStore.Data.Entities;
using mobileStore.ViewModels.Catalog.Categories;
using mobileStore.ViewModels.Catalog.Products;
using mobileStore.ViewModels.Common;

namespace mobileStore.WebApp.Models
{
    public class ProductCategoryViewModel
    {
        public CategoryVm? Category { get; set; }

        public PagedResult<ProductViewModel> Products { get; set; }
    }
}
