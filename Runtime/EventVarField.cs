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

    public abstract class EventVarField<InT, OutT, EventVarType> : EventVarField where EventVarType : EventVar<InT, OutT, EventVarType>
    {
        public static implicit operator OutT(EventVarField<InT, OutT, EventVarType> f) => f.Eval();


        [SerializeField] private OutT fallbackValue = default;
        [SerializeField] private EventVarType srcEV = default;

        public OutT Value => Eval();
        public EventVarType Source => srcEV;

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
                return ai.Evaluate();
            }

            return srcEV.Value;
        }
    }
}