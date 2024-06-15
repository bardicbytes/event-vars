
using System;
using UnityEditor.Build;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// defines an EventVar as receiving the particular type of input when invoking
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public interface IEventVarInput<TInput> : IEventVar
    {
        public System.Type InputValueType => typeof(TInput);

        public SerializableEventVarData<TInput> GetSerializableData();

        public TInput TypedStoredValue { get; }

        public void Raise(TInput value);
        //public override Type StoredValueType => typeof(TInput);
    }
}
