//alex@bardicbytes.com

namespace BardicBytes.EventVars
{
    public class StringEventVarListener : EventVarListener<string>
    {
        protected override void HandleTypedEventRaised(string data)
        {
            base.HandleTypedEventRaised(data);
        }
    }
}