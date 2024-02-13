//alex@bardicbytes.com

namespace BardicBytes.EventVars
{
    public interface IMinMax<T>
    {
        public T MinValue { get;}
        public T MaxValue { get; }

        T MinMaxClamp(T value);

        public void Raise(T data);
    }
}