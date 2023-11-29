//alex@bardicbytes.com
//why? https://www.youtube.com/watch?v=raQ3iHhE_Kk
#if UNITY_EDITOR
#endif

namespace BardicBytes.EventVars
{
    public interface IMinMax<T>
    {
        T MinValue { get; set; }
        T MaxValue { get; set; }

        T MinMaxClamp(T value);
    }
}