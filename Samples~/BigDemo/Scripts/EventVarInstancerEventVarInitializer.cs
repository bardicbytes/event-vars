// alex@bardicbytes.com

namespace BardicBytes.EventVars.BigDemo
{
    public class EventVarInstancerEventVarInitializer : EventVarInitializer<EventVarInstancerEventVar, EventVarInstancer>
    {
        protected override void RaiseEventVar() => _target.Raise(_initialValue);
    }
}