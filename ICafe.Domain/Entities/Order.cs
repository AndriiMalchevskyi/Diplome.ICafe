using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public ICollection<Product> Products { get; set; }
        public decimal OrderSummary { get; set; }
        public DiscountCard Discount { get; set; }
    }
}
