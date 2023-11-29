//alex@bardicbytes.com
using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = Prefixes.EV + "Vector3")]
    public class Vector3EventVar : GenericMinMaxEventVar<Vector3>, IMinMax<Vector3>
    {

        public override Vector3 MinMaxClamp(Vector3 val)
        {
            if (hasMax && hasMin)
                return new Vector3(Mathf.Clamp(val.x, MinValue.x, MaxValue.y), Mathf.Clamp(val.y, MinValue.y, MaxValue.y));
            else if (hasMax)
                return new Vector3(Mathf.Min(val.x, maxValue.x), Mathf.Min(val.y, maxValue.y), Mathf.Min(val.z, maxValue.z));
            else if (hasMin)
                return new Vector3(Mathf.Max(val.x, minValue.x), Mathf.Max(val.y, minValue.y), Mathf.Max(val.z, minValue.z));
            else return val;
        }

        public override Vector3 To(EventVars.EventVarInstanceData bc) => bc.Vector3Value;
#if UNITY_EDITOR
        protected override void SetInitialvalueOfInstanceConfig(Vector3 val, EventVars.EventVarInstanceData config) => config.Vector3Value = val;
#endif
    }
}