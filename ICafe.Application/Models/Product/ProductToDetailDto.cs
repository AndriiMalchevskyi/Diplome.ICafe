﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Models.Product
{
    public class ProductToDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }
    }
}
