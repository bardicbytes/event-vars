namespace BardicBytes.EventVars
{
    public interface IEventVar
    {
        public string GUID { get; }
        public System.Type StoredValueType { get; }
        public void SetInitialValue(EventVarInstanceData bc);
        public void SetStoredValue(object newStoredValue);
    }
}