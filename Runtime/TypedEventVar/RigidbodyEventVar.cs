//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{ 
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Rigidbody EventVar")]
    public class RigidbodyEventVar : EventVar<Rigidbody>
    {
        public override Rigidbody GetTypedValue(EventVarInstanceData data) => data.UnityObjectValue as Rigidbody;
    }
}