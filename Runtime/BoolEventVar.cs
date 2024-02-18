//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Bool")]

    public class BoolEventVar : EventVar<bool> 
    {
        public void Toggle() => Raise(!Value);

        public override bool GetTypedValue(EventVars.EventVarInstanceData bc) => bc.BoolValue;

        protected override void SetInstanceConfigValue(bool val, EventVars.EventVarInstanceData config) => config.BoolValue = val;
    }
}