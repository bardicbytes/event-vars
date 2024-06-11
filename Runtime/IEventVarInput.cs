
namespace BardicBytes.EventVars
{
    /// <summary>
    /// defines an EventVar as receiving the particular type of input when invoking
    /// </summary>
    /// <typeparam name="InputType"></typeparam>
    public interface IEventVarInput<InputType>
    {
        public System.Type InputValueType => typeof(InputType);

        public SerializableEventVarData<InputType> GetSerializableData();

        public object UntypedStoredValue { get; }
        public InputType TypedStoredValue { get; }
        public string GUID { get; }

        public void Raise(InputType value);
    }
}
