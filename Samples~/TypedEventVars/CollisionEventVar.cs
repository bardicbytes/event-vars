using BardicBytes.EventVars;
using UnityEngine;

[CreateAssetMenu(menuName = "BardicBytes/EventVars/Collision EventVar")]
public class CollisionEventVar : EventVar<Collision>
{
    public override Collision GetTypedValue(EventVarInstanceData data) => (Collision)data.SystemObjectValue;
}
