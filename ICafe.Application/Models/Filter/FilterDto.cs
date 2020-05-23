using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Models.Filter
{
    public class FilterDto
    {
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 20;
        public string Type { get; set; }
    }
}
