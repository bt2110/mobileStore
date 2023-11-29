using mobileStore.ViewModels.Sale;

namespace mobileStore.WebApp.Models
{
    public class CartViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public int Promotion { get; set; }
        public string DiscountCode { get; set; }
    }
}

