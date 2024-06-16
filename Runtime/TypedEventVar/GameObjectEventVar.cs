// alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/GameObject")]
    public class GameObjectEventVar : TypedEventVar<GameObject>
    {
        public override GameObject GetTypedValue(EventVarInstanceData bc) => (GameObject)bc.UnityObjectValue;
    }
}