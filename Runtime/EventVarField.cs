//alex@bardicbytes.com
//why? https://www.youtube.com/watch?v=raQ3iHhE_Kk
using UnityEngine;

namespace BardicBytes.EventVars
{
    public abstract class EventVarField
    {
        [SerializeField] protected EventVarInstancer instancer;
        public virtual void Validate()
        {
            Debug.Assert(instancer != null);
        }
    }
}