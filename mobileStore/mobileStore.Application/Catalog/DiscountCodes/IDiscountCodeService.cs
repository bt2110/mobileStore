using mobileStore.ViewModels.Catalog.Products;
using mobileStore.ViewModels.Common;
using mobileStore.ViewModels.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.Application.Catalog.DiscountCodes
{
    public interface IDiscountCodeService
    {
        Task<int> Create(DiscountCreateRequest request);

        Task<int> Update(DiscountUpdateRequest request);

        Task<int> Delete(int couponId);

        Task<PagedResult<DiscountViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<List<DiscountViewModel>> GetAll();

        Task<DiscountViewModel> GetById(int id);
    }
}
