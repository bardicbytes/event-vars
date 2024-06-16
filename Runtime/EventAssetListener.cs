// alex@bardicbytes.com

using UnityEngine;
using UnityEngine.Events;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// This component subscribes to an EventVar and invokes a UnityEvent on the game object.
    /// </summary>
    public class EventAssetListener : MonoBehaviour
    {
        public virtual string[] EditorProperties => new string[] { "untypedEventVar", "untypedResponse" };

        [Space]

        [SerializeField] protected EventAsset untypedEventVar = default;
        [SerializeField] protected UnityEvent untypedResponse = default;

        protected virtual void OnEnable() => untypedEventVar?.AddListener(HandleUntypedEventRaised);

        protected virtual void OnDisable() => untypedEventVar?.RemoveListener(HandleUntypedEventRaised);

        protected virtual void HandleUntypedEventRaised() => untypedResponse.Invoke();
    }
}