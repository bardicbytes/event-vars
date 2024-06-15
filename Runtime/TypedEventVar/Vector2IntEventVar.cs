//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Vector2Int")]
    public class Vector2IntEventVar : TypedEventVar<int>, IMinMax<Vector2Int>
    {
        [field: Header("MinMax")]
        [field: SerializeField]
        public bool HasMin { get; protected set; } = false;
        [field: SerializeField]
        public bool HasMax { get; protected set; } = false;
        
        [field: SerializeField]
        public Vector2Int MinValue { get; protected set; } = Vector2Int.zero;
        [field: SerializeField]
        public Vector2Int MaxValue { get; protected set; } = Vector2Int.one;

        public Vector2Int MinMaxClamp(Vector2Int val)
        {
            if (HasMax && HasMin)
                return new Vector2Int(Mathf.Clamp(val.x, MinValue.x, MaxValue.y), Mathf.Clamp(val.y, MinValue.y, MaxValue.y));
            else if (HasMax)
                return new Vector2Int(Mathf.Min(val.x, MaxValue.x), val.y);
            else if (HasMin)
                return new Vector2Int(val.x, Mathf.Max(val.y, MinValue.y));
            else return val;
        }

        public void Raise(Vector2Int data)
        {
            throw new System.NotImplementedException();
        }

        public override int GetTypedValue(EventVarInstanceData bc) => bc.IntValue;
    }
}