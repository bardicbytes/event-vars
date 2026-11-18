//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// this class tree is designed for the easy creation of simple scripts that initialize EventVars with values.
    /// EventVarInstancer runs its Awake first. Assign the instancer here to raise the clone instead of the shared asset.
    /// </summary>
    public abstract class EventVarInitializer : MonoBehaviour { }

    public abstract class EventVarInitializer<TEventVarType, TInputType> : EventVarInitializer
        where TEventVarType : EventAsset
    {
        [SerializeField] protected TEventVarType _target = default;
        [SerializeField] protected TInputType _initialValue = default;
        [SerializeField]
        [Tooltip("When set, Raise uses this instancer's clone of the target. The instancer's Awake runs first.")]
        protected EventVarInstancer _instancer = default;

        private void Awake() => RaiseEventVar();

        protected abstract void RaiseEventVar();

        protected TEventVarType ResolveTarget()
        {
            if (_target == null) return null;

            if (_instancer == null) return _target;

            if (Application.isPlaying && _target is BaseEventVar asset)
            {
                if (_instancer.HasInstance(asset))
                {
                    var clone = _instancer.GetInstance(asset);
                    if (clone is TEventVarType typed) return typed;
                }

                Debug.LogWarning(
                    $"{name}: Instancer '{_instancer.name}' has no instance of '{asset.name}'. Skipping raise to avoid mutating the shared asset.",
                    this);
                return null;
            }

            return _target;
        }
    }
}
