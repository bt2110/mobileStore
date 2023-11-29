using mobileStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.ViewModels.Sale
{
    public class OrderViewModel
    {
        public int Id { set; get; }
        public Guid? UserID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { set; get; }
        public OrderStatus Status { set; get; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Price { get; set; }
        public string ShipName { set; get; }
        public string ShipAddress { set; get; }
        public string ShipPhoneNumber { set; get; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
