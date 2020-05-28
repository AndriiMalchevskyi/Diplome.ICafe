using ICafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Models.Order
{
    public class OrderToUpdateDto
    {
        public int Id { get; set; }
        public ICafe.Domain.Entities.User Owner { get; set; }
        public int[] ProductsIds { get; set; }
        public decimal OrderSummary { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DiscountCard Discount { get; set; }
    }
}
