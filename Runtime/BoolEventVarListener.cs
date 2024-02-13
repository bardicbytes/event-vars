//alex@bardicbytes.com
using UnityEngine;

namespace BardicBytes.EventVars
{
    public class BoolEventVarListener : EventVarListener<bool>
    {
        [SerializeField]
        private bool invertValueForResponse = false;

        protected override void HandleTypedEventRaised(bool data)
        {
            if (invertValueForResponse) base.HandleTypedEventRaised(!data);
            else base.HandleTypedEventRaised(data);
        }
    }
}