using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Models.Product
{
    public class ProductToUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
    }
}
