using System;
using System.Diagnostics;

namespace PDE
{
    public class ArgumentEx<T>
    {
        public T Value { get; set; }
        public string Name { get; set; }
        public ArgumentEx(T value, string name)
        {
            Value = value;
            Name = name;
        }
        public static implicit operator T(ArgumentEx<T> arg)
        {
            return arg.Value;
        }
    }

    public static class ValidationExtention
    {
        public static ArgumentEx<T> RequiredThat<T>(this T arg, string name)
        {
            return new ArgumentEx<T>(arg, name);
        }

        [DebuggerHidden]
        public static ArgumentEx<T> NotNull<T>(
            this ArgumentEx<T> arg) where T : class
        {
            if (arg.Value == null)
                throw new ArgumentNullException(arg.Name);
            return arg;
        }

        [DebuggerHidden]
        public static ArgumentEx<T> NotEmpty<T>(
            this ArgumentEx<T> arg) where T : struct
        {
            if (default(T).Equals(arg.Value))
                throw new ArgumentException(arg.Name);
            return arg;
        }
    }
}
