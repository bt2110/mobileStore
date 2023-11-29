using mobileStore.ViewModels.Catalog.Categories;

namespace mobileStore.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVm>> GetAll();

        Task<CategoryVm> GetById(int id);

    }
}
