//alex@bardicbytes.com

using BardicBytes.EventVars;
using UnityEngine;

public class RigidbodyEventVarListener : EventVarListener<Rigidbody>
{
    public class TransformEventVarInitializer : EventVarInitializer<RigidbodyEventVar, Rigidbody> 
    {
        protected override void RaiseEventVar() => _target.Raise(_initialValue);
    }
}
