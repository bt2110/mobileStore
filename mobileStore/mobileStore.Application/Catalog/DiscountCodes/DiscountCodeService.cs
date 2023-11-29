using Microsoft.EntityFrameworkCore;
using mobileStore.Data.EF;
using mobileStore.Data.Entities;
using mobileStore.Utilities.Exceptions;
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
    public class DiscountCodeService : IDiscountCodeService
    {
        private readonly EShopDbContext _context;

        public DiscountCodeService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(DiscountCreateRequest request)
        {
            var discount = new DiscountCode()
            {
                Code = request.Code,
                Count = request.Count,
                Promotion = request.Promotion,
                Describe = request.Describe
            };

            _context.DiscountCodes.Add(discount);
            await _context.SaveChangesAsync();
            return discount.Id;
        }

        public async Task<int> Update(DiscountUpdateRequest request)
        {
            var discount = await _context.DiscountCodes.FindAsync(request.Id);
            if (discount == null) throw new EShopException($"Không thể tìm coupon có ID: {request.Id} ");

            discount.Code = request.Code;
            discount.Count = request.Count;
            discount.Promotion = request.Promotion;
            discount.Describe = request.Describe;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int discountId)
        {
            var discount = await _context.DiscountCodes.FindAsync(discountId);
            if (discount == null) throw new EShopException($"Không thể tìm coupon có ID: {discount} ");

            _context.DiscountCodes.Remove(discount);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<DiscountViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from c in _context.DiscountCodes
                        select new { c };

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.c.Code.Contains(request.Keyword));

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new DiscountViewModel()
                {
                    Id = x.c.Id,
                    Code = x.c.Code,
                    Count = x.c.Count,
                    Promotion = x.c.Promotion,
                    Describe = x.c.Describe
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<DiscountViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<List<DiscountViewModel>> GetAll()
        {
            var query = from c in _context.DiscountCodes
                        select new { c };

            return await query.Select(x => new DiscountViewModel()
            {
                Id = x.c.Id,
                Code = x.c.Code,
                Count = x.c.Count,
                Promotion = x.c.Promotion,
                Describe = x.c.Describe
            }).ToListAsync();
        }

        public async Task<DiscountViewModel> GetById(int id)
        {
            var query = from c in _context.DiscountCodes
                        where c.Id == id
                        select new { c };

            return await query.Select(x => new DiscountViewModel()
            {
                Id = x.c.Id,
                Code = x.c.Code,
                Count = x.c.Count,
                Promotion = x.c.Promotion,
                Describe = x.c.Describe
            }).FirstOrDefaultAsync();
        }
    }
}

