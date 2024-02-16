//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Vector2Int")]
    public class Vector2IntEventVar : MinMaxEventVar<Vector2Int>, IMinMax<Vector2Int>
    {
        public override Vector2Int MinMaxClamp(Vector2Int val)
        {
            if (hasMax && hasMin)
                return new Vector2Int(Mathf.Clamp(val.x, MinValue.x, MaxValue.y), Mathf.Clamp(val.y, MinValue.y, MaxValue.y));
            else if (hasMax)
                return new Vector2Int(Mathf.Min(val.x, MaxValue.x), val.y);
            else if (hasMin)
                return new Vector2Int(val.x, Mathf.Max(val.y, MinValue.y));
            else return val;
        }

        public override Vector2Int To(EventVarInstanceData bc) => bc.Vector2IntValue;
#if UNITY_EDITOR
        protected override void SetInstanceConfigValue(Vector2Int val, EventVarInstanceData config) => config.Vector2IntValue = val;
#endif
    }
}