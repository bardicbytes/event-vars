//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// Collision is a transient physics object and cannot be serialized into <see cref="EventVarInstanceData"/>.
    /// Instancing via EventVarInstancer is not supported for this type.
    /// </summary>
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Collision EventVar")]
    public class CollisionEventVar : TypedEventVar<Collision>
    {
        public override Collision GetTypedValue(EventVarInstanceData data) => null;
    }
}