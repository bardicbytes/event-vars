//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = Prefixes.EV + "Float")]
    public class FloatEventVar : EventVar<float>, IMinMax<float>
    {
        [Header("MinMax")]
        [SerializeField]
        protected bool hasMin = false;
        [field: SerializeField]
        public float MinValue { get; protected set; } = 0;
        [SerializeField]
        protected bool hasMax = false;
        [field: SerializeField]
        public float MaxValue { get; protected set; } = 1;

        public float MinMaxClamp(float val)
        {
            if (hasMax && hasMin)
                return Mathf.Clamp(val, MinValue, MaxValue);
            else if (hasMax)
                return Mathf.Min(val, MaxValue);
            else if (hasMin)
                return Mathf.Max(val, MinValue);
            else return val;
        }

        public override void Raise(float data) => base.Raise(MinMaxClamp(data));

        public override float To(EventVars.EventVarInstanceData bc) => bc.FloatValue;
#if UNITY_EDITOR
        protected override void SetInstanceConfigValue(float val, EventVars.EventVarInstanceData config) => config.FloatValue = val;
#endif
    }
}