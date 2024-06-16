//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Rigidbody")]
    public class RigidbodyEventVar : TypedEventVar<Rigidbody>
    {
        public override Rigidbody GetTypedValue(EventVarInstanceData data) => data.UnityObjectValue as Rigidbody;
    }
}