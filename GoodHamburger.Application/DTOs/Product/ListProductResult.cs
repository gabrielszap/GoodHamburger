using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.DTOs.Product
{
    public class ListProductResult
    {
        public List<ProductResult> Products { get; set; } = new List<ProductResult>();
    }
}
