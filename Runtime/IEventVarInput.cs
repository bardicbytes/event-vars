
namespace BardicBytes.EventVars
{
    //defines an EventVar as receiving the particular type of input when invoking
    public interface IEventVarInput<InputType>
    {
        public System.Type InputValueType => typeof(InputType);

        public SerializableEventVarData<InputType> GetSerializableData();

        public object UntypedStoredValue { get; }
        public InputType TypedStoredValue { get; }
        public string GUID { get; }
    }
}
