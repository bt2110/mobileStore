using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mobileStore.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.Application.System.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Data.Entities.Role> _roleManager;

        public RoleService(RoleManager<Data.Entities.Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleVm>> GetAll()
        {
            var roles = await _roleManager.Roles
                .Select(x => new RoleVm()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();

            return roles;
        }
    }
}
