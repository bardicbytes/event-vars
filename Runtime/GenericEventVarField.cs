//alex@bardicbytes.com
//why? https://www.youtube.com/watch?v=raQ3iHhE_Kk
using System;
using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars
{
    public abstract class GenericEventVarField<InT, OutT, EvT> : EventVarField where EvT : GenericEventVar<InT, OutT, EvT>
    {
        public static implicit operator OutT(GenericEventVarField<InT, OutT, EvT> f) => f.Eval();

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
            else if (instancer == null)
            {
                // do nothing
            }
            else if (instancer.HasInstance(srcEV))
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