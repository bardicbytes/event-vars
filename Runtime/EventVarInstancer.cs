//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// The EventVarInstancer class is responsible for managing and creating instances
    /// of EventVar objects. It acts as a factory for EventVar instances, providing
    /// functionalities to initialize, clone, and manage EventVar instances during runtime.
    /// 
    /// Responsibilities include:
    /// - Creating new instances of EventVar objects.
    /// - Initializing instances with specified values.
    /// - Managing the lifecycle of EventVar instances within the application.
    ///
    /// This class helps to encapsulate the logic around event variable instantiation,
    /// ensuring that instances are created and managed in a consistent manner.
    /// Awake runs before default components so initializers can resolve clones.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    public class EventVarInstancer : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("eventVarInstances")]
        private List<EventVarInstanceData> _eventVarInstances;

        private Dictionary<BaseEventVar, int> _evInstanceLookup = default;

        public bool IsInitialized { get; protected set; } = false;

#if UNITY_EDITOR
        [ContextMenu("Refresh Editor Names")]
        private void RefreshEditorNames()
        {
            if (_eventVarInstances == null) return;
            for (int i = 0; i < _eventVarInstances.Count; i++)
            {
                if (_eventVarInstances[i] == null) continue;
                _eventVarInstances[i].RefreshEditorName();
            }
        }
#endif

        private void Awake() => Initialize();

        private void OnDestroy()
        {
            if (_eventVarInstances == null) return;
            for (int i = 0; i < _eventVarInstances.Count; i++)
            {
                _eventVarInstances[i]?.DestroyClone();
            }
        }

        private void Initialize()
        {
            if (IsInitialized && Application.isPlaying && _evInstanceLookup != null) return;

            _evInstanceLookup = new Dictionary<BaseEventVar, int>();
            if (_eventVarInstances == null) _eventVarInstances = new List<EventVarInstanceData>();

            for (int i = 0; i < _eventVarInstances.Count; i++)
            {
                if (_eventVarInstances[i] == null || _eventVarInstances[i].Source == null) continue;

                var source = _eventVarInstances[i].Source;
                if (_evInstanceLookup.ContainsKey(source))
                {
                    Debug.LogError($"Duplicate EventVar '{source.name}' on {name}. Skipping the extra entry.", this);
                    continue;
                }

                _eventVarInstances[i].RuntimeInitialize(this);
                _evInstanceLookup.Add(source, i);
            }
            IsInitialized = true;
        }

        /// <summary>
        /// Get the instancer's copy of the argument
        /// </summary>
        /// <typeparam name="T">The EventVar subtype</typeparam>
        /// <param name="eventVarAssetRef"></param>
        /// <returns></returns>
        public T GetInstance<T>(T eventVarAssetRef) where T : BaseEventVar => GetInstance(eventVarAssetRef as BaseEventVar) as T;

        /// <summary>
        /// Get the instancer's copy of the argument
        /// </summary>
        /// <param name="eventVarAssetRef">The source asset event var of which the instancer may have a clone</param>
        /// <returns>returns null if the instancer does not have an instance of the source event var reference</returns>
        public BaseEventVar GetInstance(BaseEventVar eventVarAssetRef)
        {
            BaseEventVar ev = null;
            if (eventVarAssetRef == null)
            {
                Debug.LogWarning($"GetInstance called with null EventVar on {name}.", this);
                return null;
            }

            if (!HasInstance(eventVarAssetRef))
            {
                Debug.LogWarning($"no instance of {eventVarAssetRef.name} found on {name}, Check with HasInstance first.", this);
                return ev;
            }

            if (Application.isPlaying)
            {
                Initialize();
                var index = _evInstanceLookup[eventVarAssetRef];
                ev = _eventVarInstances[index].EventVarInstance as BaseEventVar;
            }
            else
            {
                Debug.LogWarning("GetIsntance at Editor time returns null");
                ev = null;
            }
            Debug.Assert(ev != null, "has instance, but no instance found?, but that's not cool. Initialized? " + IsInitialized);
            return ev;
        }

        /// <summary>
        /// for editor time only. use GetInstance at runtime
        /// </summary>
        /// <param name="ev">the ev asset</param>
        /// <returns>the instance data for that asset or null if no data exists</returns>
        public EventVarInstanceData FindInstanceData(BaseEventVar ev)
        {
            if (_eventVarInstances == null) return null;
            for (int i = 0; i < _eventVarInstances.Count; i++)
            {
                var entry = _eventVarInstances[i];
                if (entry != null && entry.Source == ev) return entry;
            }
            return null;
        }

        /// <summary>
        /// Check if the instancer has an instance of a given event var.
        /// </summary>
        /// <param name="ev">the event var asset that may be used as a key in this instancer</param>
        /// <returns>true if the instancer has an instance of the event var</returns>
        public bool HasInstance(BaseEventVar ev)
        {
            if (ev == null) return false;
            if (_eventVarInstances == null) return false;

            if (!Application.isPlaying)
            {
                for (int i = 0; i < _eventVarInstances.Count; i++)
                {
                    if (_eventVarInstances[i] != null && _eventVarInstances[i].Source == ev) return true;
                }
                return false;
            }

            if (!IsInitialized) Initialize();

            return _evInstanceLookup.ContainsKey(ev);
        }
    }
}
