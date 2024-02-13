//alex@bardicbytes.com
using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = Prefixes.EV + "Vector3")]
    public class Vector3EventVar : MinMaxEventVar<Vector3>, IMinMax<Vector3>
    {

        public override Vector3 MinMaxClamp(Vector3 val)
        {
            if (hasMax && hasMin)
                return new Vector3(Mathf.Clamp(val.x, MinValue.x, MaxValue.y), Mathf.Clamp(val.y, MinValue.y, MaxValue.y));
            else if (hasMax)
                return new Vector3(Mathf.Min(val.x, MaxValue.x), Mathf.Min(val.y, MaxValue.y), Mathf.Min(val.z, MaxValue.z));
            else if (hasMin)
                return new Vector3(Mathf.Max(val.x, MinValue.x), Mathf.Max(val.y, MinValue.y), Mathf.Max(val.z, MinValue.z));
            else return val;
        }

        public override Vector3 To(EventVars.EventVarInstanceData bc) => bc.Vector3Value;
#if UNITY_EDITOR
        protected override void SetInstanceConfigValue(Vector3 val, EventVars.EventVarInstanceData config) => config.Vector3Value = val;
#endif
    }
}