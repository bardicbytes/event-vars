// alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Transform")]
    public class TransformEventVar : EventVar<Transform> 
    {
        public override Transform GetTypedValue(EventVarInstanceData bc) => (Transform)bc.UnityObjectValue;
    }
}