//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Vector2")]
    public class Vector2EventVar : TypedEventVar<Vector2>, IMinMax<Vector2>
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
        public Vector2 MinValue { get; protected set; } = Vector2.zero;
        [field: SerializeField]
        public Vector2 MaxValue { get; protected set; } = Vector2.one;

        public Vector2 MinMaxClamp(Vector2 val)
        {
            if (HasMax && HasMin)
                return new Vector2(Mathf.Clamp(val.x, MinValue.x, MaxValue.x), Mathf.Clamp(val.y, MinValue.y, MaxValue.y));
            else if (HasMax)
                return new Vector2(Mathf.Min(val.x, MaxValue.x), Mathf.Min(val.y, MaxValue.y));
            else if (HasMin)
                return new Vector2(Mathf.Max(val.x, MinValue.x), Mathf.Max(val.y, MinValue.y));
            else return val;
        }

        protected override Vector2 ClampStoredValue(Vector2 value) => MinMaxClamp(value);

        public override Vector2 GetTypedValue(EventVarInstanceData bc) => bc.Vector2Value;
    }
}
