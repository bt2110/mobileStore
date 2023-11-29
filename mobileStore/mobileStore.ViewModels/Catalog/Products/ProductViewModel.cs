using mobileStore.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Display { set; get; }
        public string OperatingSystem { set; get; }
        public string Camera { set; get; }
        public string Chip { set; get; }
        public string ROM_RAM { set; get; }
        public string Connectivity { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string SeoAlias { get; set; }
        public decimal Price { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }
        public int DiscountPercentage { set; get; }
        public string ThumbnailImage { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public CategoryVm Category { get; set; }
        public List<string> Categories { get; set; } = new List<string>();

        public List<ReviewViewModel> Reviews { get; set; }


    }
}
