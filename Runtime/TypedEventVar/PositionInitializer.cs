//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    public class PositionInitializer : EventVarInitializer<Vector3EventVar, Transform>
    {
        protected override void RaiseEventVar() => _target.Raise(_initialValue.position);
    }
}
