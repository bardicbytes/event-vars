//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Vector3")]
    public class Vector2EventVar : EventVar<Vector2>, IMinMax<Vector2>
    {
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
                return new Vector2(Mathf.Clamp(val.x, MinValue.x, MaxValue.y), Mathf.Clamp(val.y, MinValue.y, MaxValue.y));
            else if (HasMax)
                return new Vector2(Mathf.Min(val.x, MaxValue.x), Mathf.Min(val.y, MaxValue.y));
            else if (HasMin)
                return new Vector2(Mathf.Max(val.x, MinValue.x), Mathf.Max(val.y, MinValue.y));
            else return val;
        }

        public override Vector2 GetTypedValue(EventVarInstanceData bc) => bc.Vector3Value;
    }
}