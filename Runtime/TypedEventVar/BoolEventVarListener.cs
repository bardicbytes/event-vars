//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    public class BoolEventVarListener : EventVarListener<bool>
    {
        public override string[] EditorProperties => new string[]
        {
            "typedEventVar",
            "instancer",
            "typedResponse",
            "invertValueForResponse",
            "invokeOnEnable",
            "conditionalResponses"
        };

        [SerializeField]
        private bool invertValueForResponse = false;

        protected override void HandleTypedEventRaised(bool data)
        {
            if (invertValueForResponse) base.HandleTypedEventRaised(!data);
            else base.HandleTypedEventRaised(data);
        }
    }
}