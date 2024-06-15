// alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// This component subscribes to an EventVar and invokes a UnityEvent on the game object.
    /// </summary>
    public class EventVarListener : MonoBehaviour
    {
        public virtual string[] EditorProperties => new string[]{"untypedEventVar", "untypedResponse" };

        [Space]

        [SerializeField] protected EventAsset untypedEventVar = default;
        [SerializeField] protected UnityEvent untypedResponse = default;

        protected virtual void OnEnable() => untypedEventVar?.AddListener(HandleUntypedEventRaised);

        protected virtual void OnDisable() => untypedEventVar?.RemoveListener(HandleUntypedEventRaised);

        protected virtual void HandleUntypedEventRaised() => untypedResponse.Invoke();
    }

    /// <summary>
    /// This base abstract class allows for easy creation of listener components of all types.
    /// </summary>
    /// <typeparam name="TOutput">the output type of the typed EventVar, not the EventVar type itself</typeparam>
    public abstract class EventVarListener<TOutput> : EventVarListener
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