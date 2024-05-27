//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Vector3")]
    public class Vector3EventVar : EventVar<Vector3>, IMinMax<Vector3>
    {
        [field: Header("MinMax")]
        [field: SerializeField]
        public bool HasMin { get; protected set; } = false;

        [field: SerializeField]
        public bool HasMax { get; protected set; } = false;

        [field: SerializeField]
        public Vector3 MinValue { get; protected set; } = Vector3.zero;
        [SerializeField]
        protected bool hasMax = false;
        [field: SerializeField]
        public Vector3 MaxValue { get; protected set; } = Vector3.one;

        public Vector3 MinMaxClamp(Vector3 val)
        {
            if (hasMax && HasMin)
                return new Vector3(Mathf.Clamp(val.x, MinValue.x, MaxValue.y), Mathf.Clamp(val.y, MinValue.y, MaxValue.y), Mathf.Clamp(val.z, MinValue.z, MaxValue.z));
            else if (hasMax)
                return new Vector3(Mathf.Min(val.x, MaxValue.x), Mathf.Min(val.y, MaxValue.y), Mathf.Min(val.z, MaxValue.z));
            else if (HasMin)
                return new Vector3(Mathf.Max(val.x, MinValue.x), Mathf.Max(val.y, MinValue.y), Mathf.Max(val.z, MinValue.z));
            else return val;
        }

        public override Vector3 GetTypedValue(EventVarInstanceData bc) => bc.Vector3Value;
    }
}