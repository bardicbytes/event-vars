//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{

    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Int")]
    public class IntEventVar : MinMaxEventVar<int>, IMinMax<int>
    {
        public override int MinMaxClamp(int val)
        {
            if (hasMax && hasMin)
                return Mathf.Clamp(val, MinValue, MaxValue);
            else if (hasMax)
                return Mathf.Min(val, MaxValue);
            else if (hasMin)
                return Mathf.Max(val, MinValue);
            else return val;
        }

        public override int To(EventVars.EventVarInstanceData bc) => bc.IntValue;
        protected override void SetInstanceConfigValue(int val, EventVars.EventVarInstanceData config) => config.IntValue = val;

        public void Increment() => Raise(Value + 1);


    }
}