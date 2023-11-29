using BardicBytes.EventVars;
using UnityEngine;

namespace BardicBytes.EventVars
{
    public abstract class GenericSystemObjectEventVar<T> : SimpleGenericEventVar<T>
    {
        public override T To(EventVarInstanceData bc) => (T)bc.SystemObjectValue;
#if UNITY_EDITOR
        protected override void SetInitialvalueOfInstanceConfig(T val, EventVarInstanceData config) => config.SystemObjectValue = val;
#endif
    }
}