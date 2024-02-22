//alex@bardicbytes.com

namespace BardicBytes.EventVars
{
    [System.Serializable]
    public class SerializableEventVarData<T>
    {
        public string guid;
        public T value;

        public SerializableEventVarData(IEventVarInput<T> ev)
        {
            this.guid = ev.GUID;
            this.value = ev.TypedStoredValue;
        }
    }
}