//alex@bardicbytes.com
using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = Prefixes.EV + "Bool")]

    public class BoolEventVar : SimpleGenericEventVar<bool>
    {
        public void Toggle() => Raise(!Value);

        public override bool To(EventVars.EventVarInstanceData bc) => bc.BoolValue;
#if UNITY_EDITOR

        protected override void SetInitialvalueOfInstanceConfig(bool val, EventVars.EventVarInstanceData config) => config.BoolValue = val;
#endif
    }
}