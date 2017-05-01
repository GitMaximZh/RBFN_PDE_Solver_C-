using System;
using System.Collections.Generic;

namespace PDE.Statistic
{
    public interface IExtendable
    {
        void AddExtention(IExtention extention);
        void ExecuteExtention<T>() where T : IExtention;
    }

    public class Extendable : IExtendable
    {
        private IDictionary<Type, IExtention> _extentions = new Dictionary<Type, IExtention>();

        public void AddExtention(IExtention extention)
        {
            if(_extentions.ContainsKey(extention.GetType()))
               _extentions[extention.GetType()] = extention;
            else
                _extentions.Add(extention.GetType(), extention);
        }

        public void ExecuteExtention<T>() where T : IExtention
        {
            foreach (var type in _extentions.Keys)
            {
                if(typeof(T).IsAssignableFrom(type))
                    _extentions[type].Execute();
            }
        }
    }

    public interface IExtention
    {
        void Execute();
    }

    public abstract class Extention<T> : IExtention where T : class, IExtendable
    {
        protected readonly T Extendable;

        public Extention(T extendable)
        {
            Extendable = extendable;
            Extendable.AddExtention(this);
        }

        public abstract void Execute();
    }
}
