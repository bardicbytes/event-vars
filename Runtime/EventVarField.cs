//alex@bardicbytes.com

using UnityEngine;
using System;

namespace BardicBytes.EventVars
{
    public abstract class EventVarField
    {
        [SerializeField] protected EventVarInstancer instancer;
        public virtual void Validate() => Debug.Assert(instancer != null);
    }

    public abstract class EventVarField<InT, OutT, EvT> : EventVarField where EvT : EventVar<InT, OutT, EvT>
    {
        public static implicit operator OutT(EventVarField<InT, OutT, EvT> f) => f.Eval();

        public Type EventVarType => typeof(EvT);
        public OutT Value => Eval();
        public EvT Source => srcEV;

        [SerializeField] private OutT fallbackValue = default;
        [SerializeField] private EvT srcEV = default;

        private OutT Eval()
        {
            if (srcEV == null)
            {
                return fallbackValue;
            }

            if (instancer != null && instancer.HasInstance(srcEV))
            {
                var ai = instancer.GetInstance(srcEV);
                if (ai == null && srcEV.RequireInstancing)
                {
                    Debug.LogWarning("failed to find instance for " + srcEV.name + " in " + instancer.name);
                }
                if (ai == null) return srcEV != null ? srcEV.Value : fallbackValue;
                return ai.Eval<InT, OutT>();
            }

            return srcEV.Value;
        }
    }
}