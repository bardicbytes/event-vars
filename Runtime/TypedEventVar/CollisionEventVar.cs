//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Collision EventVar")]
    public class CollisionEventVar : TypedEventVar<Collision>
    {
        public override Collision GetTypedValue(EventVarInstanceData data) => (Collision)data.SystemObjectValue;
    }
}