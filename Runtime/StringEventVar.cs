//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/String")]
    public class StringEventVar : EventVar<string>
    {
        public override string To(EventVarInstanceData bc) => bc.StringValue;
#if UNITY_EDITOR
        protected override void SetInstanceConfigValue(string val, EventVarInstanceData config) => config.StringValue = val;
#endif
    }
}