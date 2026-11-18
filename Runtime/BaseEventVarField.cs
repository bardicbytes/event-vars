// alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// this class exists mostly for EventVarFieldPropertyDrawer to have a non-generic target
    /// </summary>
    public abstract class BaseEventVarField
    {
        [Tooltip("Optional. During play, the field reads this instancer's clone of the source EventVar. The instancer creates the clone. If the clone is missing, the fallback value is used.")]
        [SerializeField] protected EventVarInstancer _instancer;
    }
}