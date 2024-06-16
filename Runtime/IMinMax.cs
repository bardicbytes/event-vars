//alex@bardicbytes.com

namespace BardicBytes.EventVars
{
    /// <summary>
    /// Classes that implement this interface provide a minimum maximum value as well as an implemention of claming to those values
    /// </summary>
    /// <typeparam name="T">a struct, ideally a numeric struct</typeparam>
    public interface IMinMax<T> where T : struct
    {
        public bool HasMin { get; }
        public T MinValue { get; }
        public bool HasMax { get; }
        public T MaxValue { get; }

        T MinMaxClamp(T value);
    }
}