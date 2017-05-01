using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EITProblem.Model
{
    public class Model
    {
        public ReadOnlyCollection<Side> Sides { get; private set; }
        public Model(params Side[] sides)
        {
            Sides = new List<Side>(sides).AsReadOnly();
        }
    }
}
