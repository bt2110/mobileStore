using mobileStore.ViewModels.Common;
using mobileStore.ViewModels.System.Roles;

namespace mobileStore.ApiIntegration
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleVm>>> GetAll();
    }
}
