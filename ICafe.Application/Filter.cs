using ICafe.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application
{
    public class Filter : IFilter
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
