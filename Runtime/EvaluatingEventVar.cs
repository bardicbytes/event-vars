//alex@bardicbytes.com
//why? https://www.youtube.com/watch?v=raQ3iHhE_Kk
#if UNITY_EDITOR
#endif
namespace BardicBytes.EventVars
{
    public abstract class EvaluatingEventVar<InT, OutT> : GenericEventVar<InT, OutT, EvaluatingEventVar<InT, OutT>>
    {
    }
}