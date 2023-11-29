﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mobileStore.Data.EF;
using mobileStore.Data.Entities;
using mobileStore.ViewModels.Common;
using mobileStore.ViewModels.Sale;
using mobileStore.ViewModels.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.Application.Catalog.Orders
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderService(EShopDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Create Order
        public int Create(CheckoutRequest request)
        {
            var orderDetails = new List<OrderDetail>();

            foreach (var item in request.OrderDetails)
            {
                var product = _context.Products.Find(item.ProductId);

                orderDetails.Add(new OrderDetail()
                {
                    Product = product,
                    Quantity = item.Quantity,
                });

                product.Stock -= item.Quantity;
            }

            var status = (Data.Enums.OrderStatus)1;
            var payment_method = "COD";

            if (request.PaymentMethod == PaymentMethod.CreditCard)
            {
                payment_method = "Credit Card";
                status = (Data.Enums.OrderStatus)3;
            }

            var order = new Order()
            {
                UserId = request.UserID,
                OrderDate = DateTime.Now,
                OrderDetails = orderDetails,
                Status = status,
                ShipEmail = request.Email,
                ShipAddress = request.Address,
                ShipName = request.Name,
                ShipPhoneNumber = request.PhoneNumber,
                Total = request.Total,  
                PaymentMethod = payment_method,
            };

            if (request.DiscountCodeId != 0)
            {
                var discount = _context.DiscountCodes.FirstOrDefault(x => x.Id == request.DiscountCodeId);
                discount.Count -= 1;
                order.DiscountCodeId = request.DiscountCodeId;
            }

            var result = _context.Orders.Add(order);
            _context.SaveChanges();
            return result.Entity.Id;
        }

        public async Task<ApiResult<bool>> UpdateOrderStatus(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            var status = (int)order.Status;

            switch (status)
            {
                case 1:
                    order.Status = (Data.Enums.OrderStatus)2;
                    break;

                case 2:
                    order.Status = (Data.Enums.OrderStatus)3;
                    break;

                case 3:
                    order.Status = (Data.Enums.OrderStatus)4;
                    break;
            }

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> CancelOrderStatus(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            order.Status = (Data.Enums.OrderStatus)5;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<PagedResult<OrderViewModel>> GetAllPaging(GetManageOrderPagingRequest request)
        {
            var query = from o in _context.Orders
                        join c in _userManager.Users on o.UserId equals c.Id
                        select new { o, c };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.c.UserName.Contains(request.Keyword)
                 || x.o.ShipPhoneNumber.Contains(request.Keyword));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderViewModel()
                {
                    Id = x.o.Id,
                    UserID = x.o.UserId,
                    Name = x.c.FirstName,
                    Address = x.c.Address,
                    PhoneNumber = x.c.PhoneNumber,
                    Email = x.c.Email,
                    OrderDate = x.o.OrderDate,
                    Status = (Data.Enums.OrderStatus)x.o.Status,
                    ShipName = x.o.ShipName,
                    ShipAddress = x.o.ShipAddress,
                    ShipPhoneNumber = x.o.ShipPhoneNumber,
                    Price = x.o.Total
                }).ToListAsync();

            var order = data.ToList();
            foreach (var item in order)
            {
                //item.OrderDetails = GetOrderDetails(item.Id);
            }

            //4. Select and projection
            var pagedResult = new PagedResult<OrderViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<OrderByUserViewModel> GetOrderByUser(string userId)
        {
            var Guid = new Guid(userId);

            // get user's order
            var query = from o in _context.Orders
                        join c in _userManager.Users on o.UserId equals c.Id
                        where c.Id == Guid
                        select new { o, c };

            var data = await query
                 .Select(x => new OrderViewModel()
                 {
                     Id = x.o.Id,
                     UserID = x.o.UserId,
                     Name = x.c.FirstName,
                     Address = x.c.Address,
                     PhoneNumber = x.c.PhoneNumber,
                     Email = x.c.Email,
                     OrderDate = x.o.OrderDate,
                     Status = (Data.Enums.OrderStatus)x.o.Status,
                     ShipName = x.o.ShipName,
                     ShipAddress = x.o.ShipAddress,
                     ShipPhoneNumber = x.o.ShipPhoneNumber,
                     Price = x.o.Total
                 }).ToListAsync();

            var orderList = data.ToList();

            foreach (var item in orderList)
            {
                //item.OrderDetails = GetOrderDetails(item.Id);
            }

            // get user information
            var userInformation = await _userManager.FindByIdAsync(Guid.ToString());

            var userID = Guid;
            var name = userInformation.FirstName;
            var username = userInformation.UserName;
            var address = userInformation.Address;
            var phoneNumber = userInformation.PhoneNumber;
            var email = userInformation.Email;

            var orderByUserVM = new OrderByUserViewModel()
            {
                UserID = userID,
                Name = name,
                UserName = username,
                Address = address,
                PhoneNumber = phoneNumber,
                Email = email,
                Orders = orderList,
            };

            return orderByUserVM;
        }

        public OrderViewModel GetOrderById(int orderId)
        {
            var query = from o in _context.Orders
                        select new { o };

            var orders = query.ToList();

            var order = orders.FirstOrDefault(x => x.o.Id == orderId);

            var oderVM = new OrderViewModel()
            {
                Id = order.o.Id,
                UserID = order.o.UserId,
                OrderDate = order.o.OrderDate,
                Status = (Data.Enums.OrderStatus)order.o.Status,
                ShipName = order.o.ShipName,
                ShipAddress = order.o.ShipAddress,
                ShipPhoneNumber = order.o.ShipPhoneNumber,
                Price = order.o.Total,
                OrderDetails = GetOrderDetails(order.o.Id)
            };

            return oderVM;
        }

        public List<OrderDetailViewModel> GetOrderDetails(int orderId)
        {
            var order = _context.OrderDetails.Where(x => x.OrderId == orderId);

            var orderDetails = new List<OrderDetailViewModel>();

            foreach (var item in order)
            {
                orderDetails.Add(new OrderDetailViewModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }

            return orderDetails;
        }

        public decimal GetOrderPrice(List<OrderDetailViewModel> orderDetails)
        {
            decimal price = 0;
            foreach (var item in orderDetails)
            {
                var product = _context.Products.Find(item.ProductId);
                price += product.Price * item.Quantity;
            }

            return price;
        }
    }
}
