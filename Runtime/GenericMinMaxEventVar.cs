//alex@bardicbytes.com
//why? https://www.youtube.com/watch?v=raQ3iHhE_Kk
using UnityEngine;

namespace BardicBytes.EventVars
{
    public abstract class GenericMinMaxEventVar<T> : SimpleGenericEventVar<T>
    {

        [Header("MinMax")]
        [SerializeField]
        protected bool hasMin = false;
        [SerializeField]
        protected T minValue;
        [SerializeField]
        protected bool hasMax = false;
        [SerializeField]
        protected T maxValue;

        public T MinValue
        {
            get => minValue;
            set => minValue = value;
        }
        public T MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        public abstract T MinMaxClamp(T val);

        public override void Raise(T data)
        {
            base.Raise(MinMaxClamp(data));
        }


    }
}