using GoodHamburger.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.DTOs.Order
{
    public class OrderRequest
    {
        public List<ProductRequest> Products { get; set; } = new List<ProductRequest>();
        public DateTime? OrderDate { get; set; } = DateTime.Now;
    }
}
