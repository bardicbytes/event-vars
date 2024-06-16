// alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Quaternion")]
    public class QuaternionEventVar : TypedEventVar<Quaternion>
    {
        public override Quaternion GetTypedValue(EventVarInstanceData bc) => (Quaternion)bc.QuaternionValue;
    }
}