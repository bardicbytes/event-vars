//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// this class tree is designed for the easy creation of simple scripts that initialize EventVars with values.
    /// </summary>
    public abstract class EventVarInitializer : MonoBehaviour { }

    public abstract class EventVarInitializer<TEventVarType, TInputType> : EventVarInitializer
        where TEventVarType : EventAsset
    {
        [SerializeField] protected TEventVarType _target = default;
        [SerializeField] protected TInputType _initialValue = default;

        private void Awake() => RaiseEventVar();

        protected abstract void RaiseEventVar();

    }
}
