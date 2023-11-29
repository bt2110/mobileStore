using mobileStore.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.ViewModels.Sale
{
    public class GetManageOrderPagingRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
        public string? SortOption { get; set; }
    }
}
