//alex@bardicbytes.com

namespace BardicBytes.EventVars
{
    /// <summary>
    /// This class is used by the EventVarCollection to help serialize its contents for importing and exporting
    /// </summary>
    /// <typeparam name="T"></typeparam>
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