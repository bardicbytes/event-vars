//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    public class TransformEventVarInitializer : EventVarInitializer<TransformEventVar, Transform>
    {
        protected override void RaiseEventVar() => ResolveTarget()?.Raise(_initialValue);
    }
}
