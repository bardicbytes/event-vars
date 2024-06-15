// alex@bardicbytes.com

using System;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// Use this for creating unity-serialziable asset references in UnityEngine.Object 
    /// where you don't know or don't care about the typed object interface but you need a concrete class type.
    /// </summary>
    public abstract class BaseEventVar : EventAsset, IEventVar
    {
        public abstract Type StoredValueType { get; }
        public abstract void SetInitialValue(EventVarInstanceData bc);
        public abstract void SetStoredValue(object newStoredValue);
        public bool IsClone { get; protected set; } = false;
        public EventAsset CloneSource { get; protected set; } = null;
        public EventVarInstancer Owner { get; protected set; } = null;
        public abstract T GetCreateInstance<T>(EventVarInstancer instancer) where T : BaseEventVar;
    }
}