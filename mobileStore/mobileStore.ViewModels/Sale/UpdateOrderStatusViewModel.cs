using mobileStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.ViewModels.Sale
{
    public class UpdateOrderStatusViewModel
    {
        public int OrderId { get; set; }
        public OrderStatus status { get; set; }
    }
}
