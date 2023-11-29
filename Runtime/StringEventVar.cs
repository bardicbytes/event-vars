//alex@bardicbytes.com
using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = Prefixes.EV + "String")]
    public class StringEventVar : SimpleGenericEventVar<string>
    {
        public override string To(EventVarInstanceData bc) => bc.StringValue;
#if UNITY_EDITOR
        protected override void SetInitialvalueOfInstanceConfig(string val, EventVarInstanceData config) => config.StringValue = val;
#endif
    }

}