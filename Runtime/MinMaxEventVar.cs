//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    public abstract class MinMaxEventVar<T> : EventVar<T>, IMinMax<T>
    {
        [Header("MinMax")]
        [SerializeField]
        protected bool hasMin = false;
        [field:SerializeField]
        public T MinValue { get; protected set; } = default(T);
        [SerializeField]
        protected bool hasMax = false;
        [field: SerializeField]
        public T MaxValue { get; protected set; } = default(T);

        public abstract T MinMaxClamp(T val);

        public override void Raise(T data)
        {
            base.Raise(MinMaxClamp(data));
        }
    }
}