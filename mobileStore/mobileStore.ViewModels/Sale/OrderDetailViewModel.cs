using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.ViewModels.Sale
{
    public class OrderDetailViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { set; get; }
        public string? Name { set; get; }
        public string? Category { get; set; }
        public string? ThumbnailImage { get; set; }
    }
}
