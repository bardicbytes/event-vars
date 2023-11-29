//alex@bardicbytes.com
using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = Prefixes.EV + "Float")]
    public class FloatEventVar : GenericMinMaxEventVar<float>, IMinMax<float>
    {
        public override float MinMaxClamp(float val)
        {
            if (hasMax && hasMin)
                return Mathf.Clamp(val, MinValue, MaxValue);
            else if (hasMax)
                return Mathf.Min(val, maxValue);
            else if (hasMin)
                return Mathf.Max(val, minValue);
            else return val;
        }

        public override float To(EventVars.EventVarInstanceData bc) => bc.FloatValue;
#if UNITY_EDITOR
        protected override void SetInitialvalueOfInstanceConfig(float val, EventVars.EventVarInstanceData config) => config.FloatValue = val;
#endif
    }
}