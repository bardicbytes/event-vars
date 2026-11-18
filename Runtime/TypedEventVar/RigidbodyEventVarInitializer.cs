//alex@bardicbytes.com


using UnityEngine;

namespace BardicBytes.EventVars
{
    public class RigidbodyEventVarInitializer : EventVarInitializer<RigidbodyEventVar, Rigidbody>
    {
        protected override void RaiseEventVar() => ResolveTarget()?.Raise(_initialValue);
    }
}
