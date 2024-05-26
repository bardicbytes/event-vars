//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

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
    /// </summary>
    [DisallowMultipleComponent]
    public class EventVarInstancer : MonoBehaviour
    {
        [SerializeField]
        private List<EventVarInstanceData> eventVarInstances;

        private Dictionary<EventVar, int> evInstanceLookup = default;

        public bool IsInitialized { get; protected set; } = false;

#if UNITY_EDITOR
        [ContextMenu("Refresh Editor Names")]
        private void RefreshEditorNames()
        {
            for (int i = 0; i < eventVarInstances.Count; i++)
            {
                eventVarInstances[i].RefreshEditorName();
            }
        }
#endif

        public void Awake() => Initialize();

        private void Initialize()
        {
            if (IsInitialized && Application.isPlaying && evInstanceLookup != null)
            {
                return;
            }

            evInstanceLookup = new Dictionary<EventVar, int>();

            for (int i = 0; i < eventVarInstances.Count; i++)
            {
                if (eventVarInstances[i].Source == null) continue;

                eventVarInstances[i].RuntimeInitialize(this);
                evInstanceLookup.Add(eventVarInstances[i].Source, i);
            }
            IsInitialized = true;
        }

        public T GetInstance<T>(T eventVarAssetRef) where T : EventVar => GetInstance(eventVarAssetRef as EventVar) as T;

        /// <summary>
        /// Get the instancer's copy of the argument
        /// </summary>
        /// <param name="eventVarAssetRef">The source asset event var of which the instancer may have a clone</param>
        /// <returns>returns null if the instancer does not have an instance of the source event var reference</returns>
        public EventVar GetInstance(EventVar eventVarAssetRef)
        {
            EventVar ev = null;
            if (!HasInstance(eventVarAssetRef))
            {
                Debug.LogWarning("no instance found, Check with HasInstance first.");
                return ev;
            }

            if (Application.isPlaying)
            {
                Initialize();
                var index = evInstanceLookup[eventVarAssetRef];
                ev = eventVarInstances[index].EventVarInstance as EventVar;
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
        /// <returns>the instance data for that asset</returns>
        public EventVarInstanceData FindInstanceData(EventVar ev)
        {
            EventVarInstanceData e = null;
            for (int i = 0; i < eventVarInstances.Count; i++)
            {
                e = eventVarInstances[i];
                if (e.Source != ev) continue;
            }
            return e;
        }

        /// <summary>
        /// Check if the instancer has an instance of a given event var.
        /// </summary>
        /// <param name="ev">the event var asset that may be used as a key in this instancer</param>
        /// <returns>true if the instancer has an instance of the event var</returns>
        public bool HasInstance(EventVar ev)
        {
            if (!Application.isPlaying)
            {
                for(int i = 0;i < eventVarInstances.Count;i++)
                {
                    if (eventVarInstances[i].Source == ev) return true;
                }
                return false;
            }

            if (!IsInitialized) Initialize();

            return evInstanceLookup.ContainsKey(ev);
        }
    }
}