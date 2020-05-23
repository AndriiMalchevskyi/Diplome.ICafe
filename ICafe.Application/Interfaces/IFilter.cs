using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Interfaces
{
    public interface IFilter
    {
        int Offset { get; set; }
        int Limit { get; set; }
        string Type { get; set; }
    }
}
