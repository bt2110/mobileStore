using mobileStore.Data.Enums;

namespace mobileStore.Data.Entities
{
    public class Order
    {
        public int Id { set; get; }
        public DateTime OrderDate { set; get; }
        public Guid? UserId { set; get; }
        public int? DiscountCodeId { get; set; }
        public string ShipName { set; get; }
        public string ShipAddress { set; get; }
        public string? ShipEmail { set; get; }
        public string ShipPhoneNumber { set; get; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public Enums.OrderStatus  Status { set; get; }
        public List<OrderDetail> OrderDetails { get; set; }
        public User User { get; set; }
        public DiscountCode DiscountCode { get; set; }

    }
}