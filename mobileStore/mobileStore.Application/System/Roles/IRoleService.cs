using mobileStore.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.Application.System.Role
{
    public interface IRoleService
    {
        Task<List<RoleVm>> GetAll();
    }
}
