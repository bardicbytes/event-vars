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

        [SerializeField] protected EventVar untypedEventVar = default;
        [SerializeField] protected UnityEvent untypedResponse = default;

        protected virtual void OnEnable() => untypedEventVar?.AddListener(HandleUntypedEventRaised);

        protected virtual void OnDisable() => untypedEventVar?.RemoveListener(HandleUntypedEventRaised);

        protected virtual void HandleUntypedEventRaised() => untypedResponse.Invoke();
    }

    /// <summary>
    /// This base abstract class allows for easy creation of listener components of all types.
    /// </summary>
    /// <typeparam name="T">the output type of the typed EventVar, not the EventVar type itself</typeparam>
    public abstract class EventVarListener<T> : EventVarListener
    {
        public override string[] EditorProperties => new string[] { "typedEventVar", "typedResponse", "invokeOnEnable", "conditionalResponses" };

        [System.Serializable]
        public class ConditionalResponse
        {
            public T requiredResponse;
            public EventVar<T>.OutputUnityEvent conditionalEvent;
        }

        [Space]

        [SerializeField]
        protected EventVar<T> typedEventVar = default;
        [SerializeField]
        protected EventVar<T>.OutputUnityEvent typedResponse = default;
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

        protected virtual void HandleTypedEventRaised(T data)
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