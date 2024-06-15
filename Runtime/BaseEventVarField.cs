// alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// this class exists mostly for EventVarFieldPropertyDrawer to have a non-generic target
    /// </summary>
    public abstract class BaseEventVarField
    {
        [Tooltip("[OPTIONAL] If set, the field will try to get an instanced value instead of using the source EventVar.")]
        [SerializeField] protected EventVarInstancer _instancer;
    }
}