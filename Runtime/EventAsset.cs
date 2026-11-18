// alex@bardicbytes.com

using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// The EventVar class represents a generic event variable that encapsulates a value 
    /// and provides mechanisms for event-driven programming.
    /// 
    /// The class offers:
    /// - A standardized structure for storing and managing variable values.
    /// - Methods for raising events when the value changes.
    /// - Serialization and deserialization support for saving and loading data.

    /// <summary>
    /// This serves as a base for various event variable types, enabling consistent 
    /// handling and triggering of events based on value changes. Derived classes can 
    /// specialize the type of data they handle, while inheriting the event-driven 
    /// infrastructure.
    /// </summary>
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/EventAsset (EventVar without Data)")]
    public class EventAsset : ScriptableObject
    {
        /* 
       * editor only code
       */
#if UNITY_EDITOR
        /// <summary>
        /// This property provides the array of propertyNames that the custom editor will draw in the inspector window. 
        /// Override this property and return different member names to customize the editor.
        /// </summary>
        public virtual string[] EditorProperties => new string[] { "_untypedEvent" };
        /// <summary>
        /// Similar to EditorProperties, but these properties will be drawn within a foldout
        /// </summary>
        public virtual string[] AdvancedEditorProperties => new string[] { };

        private void Reset()
        {
            Reset_first_EventVar();
            EnsureAssetGUID();
            Reset_last_EventVar();
        }

        protected virtual void OnValidate()
        {
            OnValidate_first_EventVar();
            EnsureAssetGUID();
            OnValidate_last_EventVar();
        }

        private void EnsureAssetGUID()
        {
            var path = AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(path)) return;

            var assetGuid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(assetGuid)) return;

            if (GUID != assetGuid) RefreshGUID_EditorOnly();
        }

        // extendable methods
        protected virtual void Reset_first_EventVar() { }
        protected virtual void Reset_last_EventVar() { }
        protected virtual void OnValidate_first_EventVar() { }
        protected virtual void OnValidate_last_EventVar() { }

        /// <summary>
        /// If an eventVar is copied, the cached GUID won't automatically
        /// </summary>
        [ContextMenu("RefreshGUID")]
        public void RefreshGUID_EditorOnly()
        {
            var p = AssetDatabase.GetAssetPath(this);
            GUID = AssetDatabase.AssetPathToGUID(p);
        }
#endif
        /*
        * Runtime code
        */

        /*
         * Serialized / Inspector Fields and AutoProperties
         */

        /// <summary>
        /// This GUID is a cache of the asset's GUID according to Unity's Asset Database.
        /// </summary>
        [field: HideInInspector]
        [field: SerializeField]
        public string GUID { get; private set; } = Guid.NewGuid().ToString();


        [SerializeField] protected UnityEvent _untypedEvent = default;

        /*
         * Public Accessors
         */
        /// <summary>
        /// The total number of event listeners
        /// </summary>
        /// 
        public virtual int TotalListeners => (_untypedEvent == null ? 0 : _untypedEvent.GetPersistentEventCount()) + _runtimeListenerCount;
        //public virtual Type OutputValueType => default;


        /*
         * Protected fields
         */
        protected int _runtimeListenerCount = 0;
        protected float _lastRaiseTime;
        protected bool _isInitialized = false;

        /*
         * protected/private/internal methods
         */
        protected virtual void OnEnable()
        {
            ResetRuntimeState();
            Initialize();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Builds load ScriptableObjects fresh each process start, so OnEnable is enough there.
        /// In the editor with Domain Reload disabled, OnEnable does not re-run between play sessions;
        /// this hook restores runtime state before scene Awake/OnEnable.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetEventAssetsForEditorPlayMode()
        {
            var assets = Resources.FindObjectsOfTypeAll<EventAsset>();
            for (int i = 0; i < assets.Length; i++)
            {
                var asset = assets[i];
                if (asset == null) continue;
                if (asset is BaseEventVar eventVar && eventVar.IsClone) continue;
                asset.ResetRuntimeState();
            }
        }
#endif

        /// <summary>
        /// Restores the asset to its initial runtime state.
        /// Persistent inspector listeners are kept. Runtime listeners are removed.
        /// </summary>
        public virtual void ResetRuntimeState()
        {
            if (_untypedEvent != null) _untypedEvent.RemoveAllListeners();

            _runtimeListenerCount = 0;
            _lastRaiseTime = 0;
            _isInitialized = false;
        }

        protected virtual void Initialize()
        {
            if (_isInitialized || !Application.isPlaying) return;

            _runtimeListenerCount = 0;

            _lastRaiseTime = 0;
            _isInitialized = true;
        }

        /*
         * public methods
         */

        public virtual void Raise()
        {
            _lastRaiseTime = Time.realtimeSinceStartup;

            try
            {
                _untypedEvent?.Invoke();
            }
            catch (Exception)
            {
                Debug.LogError($"[f{Time.frameCount}] EventVar \"{name}\" Exception Thrown.");
                throw;
            }
        }

        public virtual void AddListener(UnityAction action)
        {
            if (Debug.isDebugBuild) Debug.Assert(action != null);
            if (_untypedEvent == null) _untypedEvent = new UnityEvent();
            _untypedEvent.AddListener(action);
            _runtimeListenerCount++;
        }

        public virtual void RemoveListener(UnityAction action)
        {
            if (_untypedEvent == null) return;
            _untypedEvent.RemoveListener(action);
            _runtimeListenerCount--;

            if (_runtimeListenerCount < 0)
            {
                Debug.LogWarning("removing no listener?");
                _runtimeListenerCount = 0;
            }
        }

        public override string ToString() => $"[EventAsset] {name}";
    }
}