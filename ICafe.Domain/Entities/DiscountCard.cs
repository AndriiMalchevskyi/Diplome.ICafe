using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Domain.Entities
{
    public class DiscountCard
    {
        public int Id { get; set; }
        public string DiscountCode { get; set; }
        public TypeOfDiscount Type { get; set; }
        public decimal Count { get; set; }
    }
}
