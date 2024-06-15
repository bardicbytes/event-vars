//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Float")]
    public class FloatEventVar : TypedEventVar<float>, IMinMax<float>
    {
#if UNITY_EDITOR
        public override string[] EditorProperties => new string[] {
            StringUtility.GetBackingFieldName("InitialValue"),
            StringUtility.GetBackingFieldName("HasMin"),
            StringUtility.GetBackingFieldName("MinValue"),
            StringUtility.GetBackingFieldName("HasMax"),
            StringUtility.GetBackingFieldName("MaxValue"),
            "_typedEvent",
        };

#endif

        // Implementation of IMinMax<float>
        [field:Header("MinMax")]
        [field:Tooltip("When true, the argument will be change to be greater than or equal to MinValue")]
        [field: SerializeField]
        public bool HasMin { get; protected set; } = false;

        [field: SerializeField]
        public float MinValue { get; protected set; } = 0;

        [field: SerializeField]
        public bool HasMax { get; protected set; } = false;
        [field: Tooltip("When true, the argument will be change to be less than or equal to MaxValue")]
        [field: SerializeField]
        public float MaxValue { get; protected set; } = 1;

        public float MinMaxClamp(float val)
        {
            if (HasMax && HasMin)
                return Mathf.Clamp(val, MinValue, MaxValue);
            else if (HasMax)
                return Mathf.Min(val, MaxValue);
            else if (HasMin)
                return Mathf.Max(val, MinValue);
            else return val;
        }

        // override Raise and clamp the value
        public override void Raise(float data) => base.Raise(MinMaxClamp(data));

        public override float GetTypedValue(EventVarInstanceData bc) => bc.FloatValue;
    }
}