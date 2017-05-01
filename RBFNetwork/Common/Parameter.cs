using System.Collections.Generic;

namespace RBFNetwork.Common
{
    public class Parameter
    {
        private Stack<double> _oldValues = new Stack<double>();
        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                _oldValues.Push(_value);
                _value = value;
            }
        }

        public void Clear()
        {
            _oldValues.Clear();
        }

        public void Undo()
        {
            if (_oldValues.Count != 0)
            {
                _value = _oldValues.Pop();
            }
        }

        public static implicit operator double(Parameter p)
        {
            return p.Value;
        }
    }
}
