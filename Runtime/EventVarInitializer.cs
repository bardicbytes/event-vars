//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// this class tree is designed for the easy creation of simple scripts that initialize EventVars with values.
    /// </summary>
    public abstract class EventVarInitializer : MonoBehaviour { }

    public abstract class EventVarInitializer<TEventVarType, TInputType> : EventVarInitializer 
        where TEventVarType : EventVar, IEventVarInput<TInputType>
    {
        [SerializeField] protected TEventVarType target = default;
        [SerializeField] protected TInputType initialValue = default;

        private void Awake()
        {
            target.Raise(initialValue);
        }
    }
}
