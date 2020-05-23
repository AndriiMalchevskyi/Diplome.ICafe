using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Domain.Entities
{
    public class Favorites
    {
        public int Id { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
