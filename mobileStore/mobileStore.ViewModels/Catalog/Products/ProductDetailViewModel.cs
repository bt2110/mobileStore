using mobileStore.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.ViewModels.Catalog.Products
{
    public class ProductDetailViewModel
    {
        public ProductViewModel Product { get; set; }

        //public List<ReviewViewModel> Reviews { get; set; }

        public List<ReviewViewModel>? ListOfReviews { get; set; }
        public string Review { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }

        // Use to get user id when add review
        public String UserCommentId { get; set; }

        public List<ProductImageViewModel>? ProductImages { get; set; }
    }
}
