using GoodHamburger.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.DTOs.Product
{
    public class ProductRequest
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductType Type { get; set; }

    }
}
