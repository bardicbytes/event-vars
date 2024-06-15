//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/String")]
    public class StringEventVar : TypedEventVar<string>
    {
        public override string GetTypedValue(EventVarInstanceData bc) => bc.StringValue;
    }
}