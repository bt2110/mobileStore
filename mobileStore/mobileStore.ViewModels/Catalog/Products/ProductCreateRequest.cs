using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        public decimal Price { set; get; }
        public int DiscountPercentage { set; get; }
        public int Stock { set; get; }
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
        public IFormFile ThumbnailImage { get; set; }
    }
}
