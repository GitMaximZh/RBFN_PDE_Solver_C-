using System;
using System.Collections.Generic;
using System.Linq;

namespace ODE.Solver.Statistic
{
    internal class Displayer
    {
        private static readonly Displayer instance = new Displayer();

        public static Displayer Instance
        {
            get { return instance; }
        }

        private Displayer()
        {
        }
        
        private IList<string> _values = new List<string>();

        public void AddToDisplay(string value)
        {
            _values.Add(value);
        }

        public void Display()
        {
            Console.WriteLine(string.Join(", ", _values.ToArray()));
            _values.Clear();
        }
    }
}
