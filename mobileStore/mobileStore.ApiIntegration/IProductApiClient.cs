using mobileStore.Data.Entities;
using mobileStore.ViewModels.Catalog.Products;
using mobileStore.ViewModels.Common;

namespace mobileStore.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetPagings(GetManageProductPagingRequest request);

        Task<bool> CreateProduct(ProductCreateRequest request);

        Task<bool> UpdateProduct(ProductUpdateRequest request);

        Task<bool> Delete(int id);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<ProductViewModel> GetById(int id);

        Task<List<ProductViewModel>> GetNewProducts(int take);

        Task<List<ProductViewModel>> GetFeaturedProducts(int take);

        Task<List<ProductViewModel>> GetDiscountedProducts(int take);

        Task<string> AddReview(ProductDetailViewModel model);

    }
}
