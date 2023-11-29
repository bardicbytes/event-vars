// alex@bardicbytes.com

using System;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = Prefixes.EV + "EvenVar without Data")]
    public class EventVar : ScriptableObject
    {
        [SerializeField]
        protected UnityEvent untypedEvent;

        protected float lastRaiseTime;
        protected bool isInitialized = false;

        public EventVar CloneSource { get; protected set; } = null;
        public EventVarInstancer InstanceOwner { get; protected set; } = null;
        public bool IsActorInstance { get; protected set; } = false;

        public virtual bool HasValue { get => false; }
        public virtual Type StoredValueType => default;
        public virtual Type OutputValueType => default;

        public virtual object UntypedStoredValue
        {
            get => untypedStoredValue; protected set
            {
                untypedStoredValue = value;
                debug_StoredValue = untypedStoredValue + "";
            }
        }

        public virtual int TotalListeners => untypedEvent.GetPersistentEventCount() + runtimeListenerCount;

        public object untypedStoredValue;

        public string debug_StoredValue;

        [field: HideInInspector]
        [field: SerializeField]
        public string GUID { get; private set; } = null;

        protected int runtimeListenerCount = 0;

        protected virtual void Reset()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(GUID) || GUID == name)
            {
                var p = UnityEditor.AssetDatabase.GetAssetPath(this);
                GUID = UnityEditor.AssetDatabase.AssetPathToGUID(p);
            }
#endif
        }

        protected virtual void OnValidate()
        {
            lastRaiseTime = 0;
            runtimeListenerCount = 0;
#if UNITY_EDITOR

            if (GUID == null)
            {
                var p = UnityEditor.AssetDatabase.GetAssetPath(this);
                GUID = UnityEditor.AssetDatabase.AssetPathToGUID(p);
            }
#endif
        }

        protected virtual void OnEnable()
        {
            if (untypedEvent != null)
                untypedEvent.RemoveAllListeners();
            runtimeListenerCount = 0;

            isInitialized = false;
            Initialize();
        }

        public virtual T Clone<T>(EventVarInstancer owner) where T : EventVar
        {
            var c = Instantiate(this) as T;
            c.CloneSource = this;
            c.InstanceOwner = owner;
            c.IsActorInstance = true;
            return c;
        }

        protected virtual void Initialize()
        {
            if (isInitialized || !Application.isPlaying) return;
            lastRaiseTime = 0;
            isInitialized = true;
        }


        public virtual OutT Eval<InT, OutT>()
        {
            return Eval<InT, OutT>((InT)UntypedStoredValue);
        }
        public virtual OutT Eval<InT, OutT>(InT inValue)
        {
            return ((GenericEventVar<InT, OutT, EvaluatingEventVar<InT, OutT>>)this).Eval(inValue);
        }

        public override string ToString()
        {
            return name + "? " + UntypedStoredValue;
        }

        public virtual void Raise()
        {
            Initialize();
            lastRaiseTime = Time.realtimeSinceStartup;

            try
            {
                untypedEvent.Invoke();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(name + "");
                throw ex;
            }
        }

        public virtual void AddListener(UnityAction action)
        {
            Initialize();

            Debug.Assert(action != null);
            untypedEvent.AddListener(action);
            runtimeListenerCount++;
        }

        public virtual void RemoveListener(UnityAction action)
        {
            Initialize();
            untypedEvent.RemoveListener(action);
            runtimeListenerCount--;
            if (runtimeListenerCount <= 0) Debug.LogWarning("removing no listener");
        }

        public virtual void SetInitialValue(EventVarInstanceData bc)
        {
            throw new NotImplementedException("There's no reason to instance an event var without without data. " + this.name);
        }

        public virtual EventVarInstanceData CreateInstanceConfig()
        {
            throw new NotImplementedException("There's no reason to instance an event var without without data." + this.name);
        }

#if UNITY_EDITOR

        public virtual void SetInitValueOfInstanceConfig(SerializedProperty prop, EventVarInstanceData config)
        {
            throw new NotImplementedException("There's no reason to instance an event var without without data." + this.name);
        }

        //public virtual void SetInitialValue(SerializedProperty prop)
        //{
        //    throw new NotImplementedException("There's no reason to instance an event var without without data.");
        //}

#endif

    }
}