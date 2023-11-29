using mobileStore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.Data.Entities
{
   
    public class DiscountCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Count { get; set; }
        public int Promotion { get; set; }
        public string Describe { get; set; }
        public List<Order> Orders { get; set; }
    }
}
