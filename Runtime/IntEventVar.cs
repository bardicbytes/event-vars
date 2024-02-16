//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{

    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Int")]
    public class IntEventVar : EventVar<int>, IMinMax<int>
    {
        [Header("MinMax")]
        [SerializeField]
        protected bool hasMin = false;
        [field: SerializeField]
        public int MinValue { get; protected set; } = 0;
        [SerializeField]
        protected bool hasMax = false;
        [field: SerializeField]
        public int MaxValue { get; protected set; } = 1;

        public int MinMaxClamp(int val)
        {
            if (hasMax && hasMin)
                return Mathf.Clamp(val, MinValue, MaxValue);
            else if (hasMax)
                return Mathf.Min(val, MaxValue);
            else if (hasMin)
                return Mathf.Max(val, MinValue);
            else return val;
        }

        public override int To(EventVarInstanceData bc) => bc.IntValue;
        protected override void SetInstanceConfigValue(int val, EventVars.EventVarInstanceData config) => config.IntValue = val;

        public void Increment() => Raise(base.Value + 1);
    }
}