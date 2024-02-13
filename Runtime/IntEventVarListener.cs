//alex@bardicbytes.com

namespace BardicBytes.EventVars
{
    public class IntEventVarListener : EventVarListener<int>
    {
        protected override void HandleTypedEventRaised(int data) => base.HandleTypedEventRaised(data);
    }
}