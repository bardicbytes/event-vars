//alex@bardicbytes.com
//why? https://www.youtube.com/watch?v=raQ3iHhE_Kk
#if UNITY_EDITOR
#endif
namespace BardicBytes.EventVars
{
    public abstract class AutoEvaluatingEventVar<InT, OutT> : EvaluatingEventVar<InT, OutT> where InT : OutT
    {
        public override OutT Eval(InT val) => val; //a lil shortcut
    }
}