using mobileStore.ViewModels.Catalog.Products;
using mobileStore.ViewModels.Common;
using mobileStore.ViewModels.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.ApiIntegration
{
    public interface IDiscountCodeApiClient
    {
        Task<List<DiscountViewModel>> GetAll();

        Task<DiscountViewModel> GetById(int id);

        Task<bool> Create(DiscountCreateRequest request);

        Task<bool> Update(DiscountUpdateRequest request);

        Task<bool> Delete(int id);

        Task<PagedResult<DiscountViewModel>> GetAllPaging(GetManageProductPagingRequest request);
    }
}
