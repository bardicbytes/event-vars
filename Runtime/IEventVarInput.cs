
namespace BardicBytes.EventVars
{
    public interface IEventVarInput<InputType>
    {
        public System.Type InputValueType => typeof(InputType);

        public SerializableEventVarData<InputType> GetSerializableData();

        public object UntypedStoredValue { get; }
        public InputType TypedStoredValue { get; }
        public string GUID { get; }
    }
}
