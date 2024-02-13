//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = Prefixes.EV + "Bool")]

    public class BoolEventVar : EventVar<bool> 
    {
        public void Toggle() => Raise(!Value);

        public override bool To(EventVars.EventVarInstanceData bc) => bc.BoolValue;
#if UNITY_EDITOR

        protected override void SetInstanceConfigValue(bool val, EventVars.EventVarInstanceData config) => config.BoolValue = val;
#endif
    }
}