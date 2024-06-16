//alex@bardicbytes.com


using UnityEngine;

namespace BardicBytes.EventVars
{
    public class RigidbodyEventVarInitializer : EventVarInitializer<RigidbodyEventVar, Rigidbody>
    {
        protected override void RaiseEventVar() => _target.Raise(_initialValue);
    }
}
