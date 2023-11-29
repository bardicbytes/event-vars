//alex@bardicbytes.com
#pragma warning disable 414
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BardicBytes.EventVars
{

    public abstract class GenericEventVarListener<DataType> : EventVarListener
    {
        [System.Serializable]
        public class ConditionalResponse
        {
            public DataType requiredResponse;
            public SimpleGenericEventVar<DataType>.UnityEvent cEvent;
        }

        [Space]

        [SerializeField]
        protected SimpleGenericEventVar<DataType> typedEventVar = default;
        [SerializeField]
        protected SimpleGenericEventVar<DataType>.UnityEvent typedResponse = default;
        [SerializeField]
        private bool invokeUntypedForDataRaise = true;
        [SerializeField]
        private bool invokeOnEnable = false;

        [SerializeField]
        protected List<ConditionalResponse> conditionalResponses = default;

        protected override void OnEnable()
        {
            if (untypedEventVar != null)
                untypedEventVar.AddListener(HandleUntypedEventRaised);
            if (typedEventVar != null)
            {
                typedEventVar.AddListener(HandleTypedEventRaised);
                if (this.invokeOnEnable) HandleTypedEventRaised(typedEventVar.Value);
            }
            else if (untypedEventVar == null)
            {
                Debug.LogWarning("Both typed and untyped event var fields are not set. " + name, this);
            }
        }

        protected override void OnDisable()
        {
            if (untypedEventVar != null)
                untypedEventVar.RemoveListener(HandleUntypedEventRaised);

            if (typedEventVar != null)
            {
                typedEventVar.RemoveListener(HandleTypedEventRaised);
            }
        }

        protected virtual void HandleTypedEventRaised(DataType data)
        {
            if (invokeUntypedForDataRaise)
                HandleUntypedEventRaised();

            typedResponse.Invoke(data);
            for (int i = 0; i < conditionalResponses.Count; i++)
            {
                if (conditionalResponses[i].requiredResponse.Equals(data))
                    conditionalResponses[i].cEvent.Invoke(data);
            }
        }
    }

}