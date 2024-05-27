//alex@bardicbytes.com
namespace BardicBytes.EventVars
{
    public class FloatEventVarListener : EventVarListener<float>
    {
        protected override void HandleTypedEventRaised(float data) => base.HandleTypedEventRaised(data);
    }
}