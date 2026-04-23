using GoodHamburger.Application.DTOs.Product;

namespace GoodHamburger.Application.DTOs.Order
{
    public class OrderResult
    {
        public Guid Id { get; set; }
        public List<ProductResult> Products { get; set; } = new List<ProductResult>();  
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
