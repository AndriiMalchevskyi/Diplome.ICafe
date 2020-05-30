using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public Photo Photo { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
