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
        public override string[] EditorProperties => new string[] { "typedEventVar", "instancer", "typedResponse", "invokeOnEnable", "conditionalResponses" };

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
        [Tooltip("When set during play, this listener subscribes to the instancer's clone of the EventVar.")]
        protected EventVarInstancer instancer = default;
        [SerializeField]
        protected UnityEvent<TOutput> typedResponse = default;
        [SerializeField]
        private bool invokeOnEnable = false;

        [SerializeField]
        protected List<ConditionalResponse> conditionalResponses = new List<ConditionalResponse>();

        private TypedEventVar<TOutput> _subscribed;

        protected override sealed void OnEnable()
        {
            _subscribed = ResolveSubscription();
            _subscribed?.AddListener(HandleTypedEventRaised);
            if (invokeOnEnable && _subscribed != null) HandleTypedEventRaised(_subscribed.Value);
        }

        protected override sealed void OnDisable()
        {
            _subscribed?.RemoveListener(HandleTypedEventRaised);
            _subscribed = null;
        }

        protected virtual void HandleTypedEventRaised(TOutput data)
        {
            typedResponse?.Invoke(data);

            if (conditionalResponses == null) return;

            for (int i = 0; i < conditionalResponses.Count; i++)
            {
                if (conditionalResponses[i] == null) continue;
                if (!EventVarEquality.Equals(conditionalResponses[i].requiredResponse, data)) continue;

                conditionalResponses[i].conditionalEvent?.Invoke(data);
            }
        }

        private TypedEventVar<TOutput> ResolveSubscription()
        {
            if (typedEventVar == null) return null;
            if (Application.isPlaying && instancer != null && instancer.HasInstance(typedEventVar))
            {
                var clone = instancer.GetInstance(typedEventVar);
                if (clone != null) return clone;
            }
            return typedEventVar;
        }
    }
}
