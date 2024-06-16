// alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars.BigDemo
{
    [CreateAssetMenu(menuName = "BardicBytes/Demo/EventVarInstancer")]
    public class EventVarInstancerEventVar : TypedEventVar<EventVarInstancer>
    {
        public override EventVarInstancer GetTypedValue(EventVarInstanceData data) => data.UnityObjectValue as EventVarInstancer;
    }
}