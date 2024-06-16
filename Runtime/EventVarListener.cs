// alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// This base abstract class allows for easy creation of listener components of all types.
    /// </summary>
    /// <typeparam name="TOutput">the output type of the typed EventVar, not the EventVar type itself</typeparam>
    public abstract class EventVarListener<TOutput> : EventAssetListener
    {
        public override string[] EditorProperties => new string[] { "typedEventVar", "typedResponse", "invokeOnEnable", "conditionalResponses" };

        [System.Serializable]
        public class ConditionalResponse
        {
            public TOutput requiredResponse;
            public UnityEvent<TOutput> conditionalEvent;
        }

        [Space]

        [SerializeField]
        protected TypedEventVar<TOutput> typedEventVar = default;
        [SerializeField]
        protected UnityEvent<TOutput> typedResponse = default;
        [SerializeField]
        private bool invokeOnEnable = false;

        [SerializeField]
        protected List<ConditionalResponse> conditionalResponses = default;

        private bool isQuitting = false;

        private void OnApplicationQuit()
        {
            isQuitting = true;
        }

        protected override sealed void OnEnable()
        {
            typedEventVar?.AddListener(HandleTypedEventRaised);
            if (this.invokeOnEnable) HandleTypedEventRaised(typedEventVar.Value);
        }

        protected override sealed void OnDisable()
        {
            if (isQuitting) return;
            typedEventVar?.RemoveListener(HandleTypedEventRaised);
        }

        protected virtual void HandleTypedEventRaised(TOutput data)
        {
            typedResponse.Invoke(data);

            for (int i = 0; i < conditionalResponses.Count; i++)
            {
                if (!conditionalResponses[i].requiredResponse.Equals(data)) continue;

                conditionalResponses[i].conditionalEvent.Invoke(data);
            }
        }
    }
}