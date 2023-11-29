//alex@bardicbytes.com
using UnityEngine;

namespace BardicBytes.EventVars
{

    [CreateAssetMenu(menuName = Prefixes.EV + "Int")]
    public class IntEventVar : GenericMinMaxEventVar<int>, IMinMax<int>
    {
        public override int MinMaxClamp(int val)
        {
            if (hasMax && hasMin)
                return Mathf.Clamp(val, MinValue, MaxValue);
            else if (hasMax)
                return Mathf.Min(val, maxValue);
            else if (hasMin)
                return Mathf.Max(val, minValue);
            else return val;
        }

        public override int To(EventVars.EventVarInstanceData bc) => bc.IntValue;
#if UNITY_EDITOR
        protected override void SetInitialvalueOfInstanceConfig(int val, EventVars.EventVarInstanceData config) => config.IntValue = val;
#endif

        public void Increment() => Raise(Value + 1);


    }
}