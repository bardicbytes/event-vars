//alex@bardicbytes.com

using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Float")]
    public class FloatEventVar : EventVar<float>, IMinMax<float>
    {
#if UNITY_EDITOR
        public override string[] EditorProperties => new string[] {
            StringFormatting.GetBackingFieldName("InitialValue"),
            StringFormatting.GetBackingFieldName("HasMin"),
            StringFormatting.GetBackingFieldName("MinValue"),
            StringFormatting.GetBackingFieldName("HasMax"),
            StringFormatting.GetBackingFieldName("MaxValue"),
            "typedEvent",
        };

#endif

        [field:Header("MinMax")]
        [field: SerializeField]
        public bool HasMin { get; protected set; } = false;

        [field: SerializeField]
        public float MinValue { get; protected set; } = 0;

        [field: SerializeField]
        public bool HasMax { get; protected set; } = false;

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

        public override void Raise(float data) => base.Raise(MinMaxClamp(data));

        public override float GetTypedValue(EventVarInstanceData bc) => bc.FloatValue;

    }
}