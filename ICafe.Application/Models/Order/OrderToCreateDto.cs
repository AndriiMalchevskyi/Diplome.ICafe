using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Models.Order
{
    public class OrderToCreateDto
    {
        public IDictionary<int, int> ProductsIds { get; set; }
        public decimal OrderSummary { get; set; }
        public string Description { get; set; }
        public ICafe.Domain.Entities.DiscountCard Discount { get; set; }
    }

}
