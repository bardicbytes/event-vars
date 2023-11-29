using BardicBytes.EventVars;
using UnityEngine;

namespace BardicBytes.EventVars
{
    public abstract class GenericUnityObjectEventVar<T> : SimpleGenericEventVar<T> where T : UnityEngine.Object
    {
        public override T To(EventVarInstanceData bc) => (T)bc.UnityObjectValue;
#if UNITY_EDITOR
        protected override void SetInitialvalueOfInstanceConfig(T val, EventVarInstanceData config) => config.UnityObjectValue = val;
#endif
    }
}