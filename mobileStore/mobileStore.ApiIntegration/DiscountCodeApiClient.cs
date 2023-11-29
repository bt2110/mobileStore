using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using mobileStore.Utilities.Constants;
using mobileStore.ViewModels.Catalog.Products;
using mobileStore.ViewModels.Common;
using mobileStore.ViewModels.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.ApiIntegration
{
    public class DiscountCodeApiClient  : BaseApiClient, IDiscountCodeApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DiscountCodeApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PagedResult<DiscountViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<DiscountViewModel>>(
               $"/api/DiscountCodes/paging?pageIndex={request.PageIndex}" +
               $"&pageSize={request.PageSize}" +
               $"&keyword={request.Keyword}&sortOption={request.SortOption}");

            return data;
        }

        public async Task<List<DiscountViewModel>> GetAll()
        {
            return await GetListAsync<DiscountViewModel>("/api/DiscountCodes");
        }

        public async Task<DiscountViewModel> GetById(int id)
        {
            return await GetAsync<DiscountViewModel>($"/api/DiscountCodes/{id}");
        }

        public async Task<bool> Create(DiscountCreateRequest request)
        {
            var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/DiscountCodes/", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(DiscountUpdateRequest request)
        {

            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/DiscountCodes/updateDiscount", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var sessions = _httpContextAccessor
                                       .HttpContext
                                       .Session
                                       .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.DeleteAsync($"/api/DiscountCodes/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
