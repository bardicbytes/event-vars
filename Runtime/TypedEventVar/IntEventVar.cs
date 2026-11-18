//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Int")]
    public class IntEventVar : TypedEventVar<int>, IMinMax<int>
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

        [field: Header("MinMax")]
        [field: SerializeField]
        public bool HasMin { get; protected set; } = false;

        [field: SerializeField]
        public bool HasMax { get; protected set; } = false;
        [field: SerializeField]
        public int MinValue { get; protected set; } = 0;
        [field: SerializeField]
        public int MaxValue { get; protected set; } = 1;

        public int MinMaxClamp(int val)
        {
            if (HasMax && HasMin)
                return Mathf.Clamp(val, MinValue, MaxValue);
            else if (HasMax)
                return Mathf.Min(val, MaxValue);
            else if (HasMin)
                return Mathf.Max(val, MinValue);
            else return val;
        }

        protected override int ClampStoredValue(int value) => MinMaxClamp(value);

        public override int GetTypedValue(EventVarInstanceData bc) => bc.IntValue;

        public void Increment() => Raise(base.Value + 1);
    }
}