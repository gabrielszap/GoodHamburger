using GoodHamburger.Application.DTOs.Product;

namespace GoodHamburger.Application.DTOs.Order
{
    public class OrderResult
    {
        public Guid Id { get; set; }
        public List<ProductResult> Products { get; set; } = new List<ProductResult>();

        public decimal Subtotal { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
