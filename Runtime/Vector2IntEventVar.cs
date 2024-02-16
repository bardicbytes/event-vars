//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Vector2Int")]
    public class Vector2IntEventVar : EventVar<int>, IMinMax<Vector2Int>
    {
        [SerializeField]
        protected bool hasMin = false;
        [field: SerializeField]
        public Vector2Int MinValue { get; protected set; } = Vector2Int.zero;
        [SerializeField]
        protected bool hasMax = false;
        [field: SerializeField]
        public Vector2Int MaxValue { get; protected set; } = Vector2Int.one;

        public Vector2Int MinMaxClamp(Vector2Int val)
        {
            if (hasMax && hasMin)
                return new Vector2Int(Mathf.Clamp(val.x, MinValue.x, MaxValue.y), Mathf.Clamp(val.y, MinValue.y, MaxValue.y));
            else if (hasMax)
                return new Vector2Int(Mathf.Min(val.x, MaxValue.x), val.y);
            else if (hasMin)
                return new Vector2Int(val.x, Mathf.Max(val.y, MinValue.y));
            else return val;
        }

        public void Raise(Vector2Int data)
        {
            throw new System.NotImplementedException();
        }

        public override int To(EventVarInstanceData bc) => bc.IntValue;
#if UNITY_EDITOR
        protected override void SetInstanceConfigValue(int val, EventVarInstanceData config) => config.IntValue = val;
#endif
    }
}