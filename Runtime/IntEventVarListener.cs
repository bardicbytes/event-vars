//alex@bardicbytes.com

namespace BardicBytes.EventVars
{
    public class IntEventVarListener : GenericEventVarListener<int>
    {
        protected override void HandleTypedEventRaised(int data) => base.HandleTypedEventRaised(data);
    }
}