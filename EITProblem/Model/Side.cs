using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EITProblem.Model
{
    public class Side
    {
        public ReadOnlyCollection<Part> Parts { get; private set; }
        public Side(params Part[] parts)
        {
            Parts = new List<Part>(parts).AsReadOnly();
        }
    }
}
