namespace mobileStore.Data.Entities
{
    public class Product
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
        public int DiscountPercentage { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }
        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Cart> Carts { get; set; }
        public List<FavoriteProduct> FavoriteProducts { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public ICollection<Review> Reviews { get; set; }


    }
}