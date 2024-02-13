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
        public virtual string[] EditorProperties => new string[] { "untypedEvent"};

        [field: HideInInspector]
        [field: SerializeField]
        public string GUID { get; private set; } = null;

        /*
         * Serialized / Inspector Fields and AutoProperties
         */
        public EventVar CloneSource { get; protected set; } = null;
        public EventVarInstancer InstanceOwner { get; protected set; } = null;
        public bool IsActorInstance { get; protected set; } = false;
        public virtual object UntypedStoredValue { get; protected set; }

        [SerializeField]
        protected UnityEvent untypedEvent;

        /*
         * Accessors
         */
        public virtual int TotalListeners => untypedEvent.GetPersistentEventCount() + runtimeListenerCount;
        public virtual bool HasValue { get => false; }
        public virtual Type StoredValueType => default;
        public virtual Type OutputValueType => default;

        /*
         * protected / private fields
         */
        protected int runtimeListenerCount = 0;
        protected float lastRaiseTime;
        protected bool isInitialized = false;

        /*
         * protected / private methods
         */

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            if (string.IsNullOrEmpty(GUID) || GUID == name) RefreshGUID();
        }

        protected virtual void OnValidate()
        {
            lastRaiseTime = 0;
            runtimeListenerCount = 0;

            if (GUID != null) return;
            RefreshGUID();
        }

        [ContextMenu("RefreshGUID")]
        private void RefreshGUID()
        {
            var p = AssetDatabase.GetAssetPath(this);
            GUID = AssetDatabase.AssetPathToGUID(p);
        }

        [MenuItem(itemName: "Bardic/Refresh All GUIDs")]
        private static void RefreshAllGUIDs()
        {
            string[] foundAssetGUIDs = AssetDatabase.FindAssets("t:BaseEventVar");
            try
            {
                for (int i = 0; i < foundAssetGUIDs.Length; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(foundAssetGUIDs[i]);
                    var asset = AssetDatabase.LoadAssetAtPath<EventVar>(path);
                    if (asset.GUID != foundAssetGUIDs[i])
                    {
                        Debug.Log($"{path} had incorrect GUID recorded. Fixed.");
                        asset.GUID = foundAssetGUIDs[i];
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        public virtual void SetInstanceConfigValue(SerializedProperty prop, EventVarInstanceData config)
        {
            throw new NotImplementedException("There's no reason to instance an event var without without data." + this.name);
        }
#endif

        protected virtual void OnEnable()
        {
            if (untypedEvent != null) untypedEvent.RemoveAllListeners();

            runtimeListenerCount = 0;
            isInitialized = false;

            Initialize();
        }

        protected virtual void Initialize()
        {
            if (isInitialized || !Application.isPlaying) return;

            lastRaiseTime = 0;
            isInitialized = true;
        }

        /*
         * public methods
         */
        public virtual OutT Eval<InT, OutT>() => Eval<InT, OutT>((InT)UntypedStoredValue);
        public virtual OutT Eval<InT, OutT>(InT inValue) => Eval<InT, OutT>(inValue);

        public virtual T GetCreateActorInstance<T>(EventVarInstancer owner) where T : EventVar
        {
            var c = Instantiate(this) as T;
            c.CloneSource = this;
            c.InstanceOwner = owner;
            c.IsActorInstance = true;
            return c;
        }

        public virtual void Raise()
        {
            Initialize();
            lastRaiseTime = Time.realtimeSinceStartup;

            try
            {
                untypedEvent.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[f{Time.frameCount}] EventVar \"{name}\" Exception Thrown.");
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

            if (runtimeListenerCount < 0)
            {
                Debug.LogWarning("removing no listener?");
                runtimeListenerCount = 0;
            }
        }

        public virtual void SetInitialValue(EventVarInstanceData bc)
        {
            throw new NotImplementedException("There's no reason to instance an event var without without data. " + this.name);
        }

        public virtual EventVarInstanceData CreateInstanceConfig()
        {
            throw new NotImplementedException("There's no reason to instance an event var without without data." + this.name);
        }
        public override string ToString() => name + "? " + UntypedStoredValue;
    }

    /// <summary>
    /// A convenience implementation of AutoEventVar
    /// </summary>
    /// <typeparam name="T">Any AutoEventVar</typeparam>
    public abstract class EventVar<T> : EventVar<T, T> { }

    public abstract class EventVar<InT, OuT> : EventVar<InT, OuT, EventVar<InT, OuT>> where InT : OuT
    {
        /// <summary>
        /// Override of EvaluatingEventVar.Eval.
        /// </summary>
        /// <param name="val">The value to evaluate</param>
        /// <returns>val, the value passed, unchanged</returns>
        public override OuT Eval(InT val) => val;
    }

    public abstract class EventVar<InT, OutT, EvT> : EventVar where EvT : EventVar<InT, OutT, EvT>
    {
        public override string[] EditorProperties => new string[] { StringFormatting.GetBackingFieldName("InitialValue"),"typedEvent", "requireData", "resetValueOnDatalessRaise","invokeNewListeners", "abortRaiseForIdenticalData" };

        public static implicit operator OutT(EventVar<InT, OutT, EvT> ev) => ev.Value;

        /// <summary>
        /// The Actor's instance Value, the source asset's Value, or this field's fallback value; in that order.
        /// </summary>
        [Serializable]
        public class Field : EventVarField<InT, OutT, EvT> { } 

        [Serializable]
        public class UnityEvent : UnityEvent<OutT> { }

        [SerializeField]
        protected UnityEvent typedEvent = default;

        [Space]
        [SerializeField]
        protected bool debugLogValueChange = false;

        [field: SerializeField, Tooltip("If TRUE, will error if value of original is accessed.")]
        public bool RequireInstancing { get; protected set; }

        [Space]
        [SerializeField, Tooltip("If TRUE, raising this EventVar with the parameterless override will reset the value of this EventVar to it's initial state.")]
        protected bool resetValueOnDatalessRaise = true;
        [SerializeField, Tooltip("If TRUE, the event will not raise if the data is identical.")]
        protected bool abortRaiseForIdenticalData = false;
        [SerializeField]
        [Tooltip("Also prevents reset/dataless raises if current value matches initial value")]
        protected bool requireData = false;
        [SerializeField]
        protected bool invokeNewListeners = false;

        [field: SerializeField]
        public InT InitialValue { get; protected set; }
        public InT StoredValue
        {
            get
            {
                var b = base.UntypedStoredValue;
                InT v = default;
                if (b != null) v = (InT)b;
                return v;
            }
        }
        public OutT Value
        {
            get
            {
                Debug.Assert(!RequireInstancing || IsActorInstance, name + " " + (IsActorInstance ? "Is ActorInst" : "Not ActorInst"));
                return Eval(StoredValue);
            }
        }
        public override bool HasValue { get => true; }
        public override Type StoredValueType => typeof(InT);
        public override Type OutputValueType => typeof(OutT);

        public override int TotalListeners => base.TotalListeners + typedEvent.GetPersistentEventCount();

#if UNITY_EDITOR
        public InT To(SerializedProperty prop) => To((prop.boxedValue as EventVarInstanceData));

        public override void SetInstanceConfigValue(SerializedProperty prop, EventVarInstanceData config)
        {
            Debug.Assert(config != null);
            var val = To(prop);
            SetInstanceConfigValue(val, config);
        }


        public virtual InT PropField(Rect position, UnityEditor.SerializedProperty rawProp)
        {
            EditorGUI.LabelField(position, InitialValue.ToString());
            return default;
        }
#endif
        protected abstract void SetInstanceConfigValue(InT val, EventVarInstanceData config);

        protected override void OnValidate()
        {
            if (StoredValue == null
                || (StoredValue != null
                && !StoredValue.Equals(InitialValue)))
            {
                UntypedStoredValue = InitialValue;
            }

            base.OnValidate();
        }

        protected override void OnEnable()
        {
            UntypedStoredValue = InitialValue;
            isInitialized = false;

            if (typedEvent != null) typedEvent.RemoveAllListeners();

            base.OnEnable();
        }

        public virtual OutT Eval(InT val) => base.Eval<InT, OutT>(val);

        /// <summary>
        /// Just use the Value property.
        /// </summary>
        /// <returns>this.Value</returns>
        public OutT Eval() => Value;

        protected override void Initialize()
        {
            if (isInitialized || !Application.isPlaying) return;
            base.Initialize();

            SetInitialValue(InitialValue);
        }

        public override string ToString() => GetValueString();

        // seprate for extensibility purposes
        public virtual string GetValueString() => StoredValue == null ? "null value" : StoredValue.ToString();

        /// <summary>
        /// Clones the source EventVar asset, and puts it in a config
        /// </summary>
        /// <returns>A new EventvarInstanceConfig upcast as a BaseConfig</returns>
        public override EventVarInstanceData CreateInstanceConfig() => new EventVarInstanceData(this);

        public override void Raise()
        {
            Initialize();
            
            Debug.Assert(!RequireInstancing || IsActorInstance);
            
            if (requireData
                && resetValueOnDatalessRaise
                && StoredValue.Equals(InitialValue)) return;
            
            if (resetValueOnDatalessRaise) ChangeCurrentValue(InitialValue);
            
            if (requireData) return;

            this.Raise(InitialValue);
        }

        public virtual void Raise(InT data)
        {
            Initialize();
            Debug.Assert(!RequireInstancing || IsActorInstance);

            if (abortRaiseForIdenticalData && StoredValue.Equals(data)) return;

            ChangeCurrentValue(data);

            try
            {
                typedEvent.Invoke(Value);
            }
            catch(Exception ex)
            {
                if (Debug.isDebugBuild) Debug.LogError($"[f{Time.frameCount}] EventVar \"{name}\" Exception Thrown in UnityEvent-{Value.GetType().Name} ({typedEvent}).");
                throw ex;
            }
        }

        private void ChangeCurrentValue(InT data)
        {
            if (Debug.isDebugBuild && debugLogValueChange && !data.Equals(StoredValue)) Debug.Log($"{name} value changes. {StoredValue}={data}");
            UntypedStoredValue = data;
        }

        public override void AddListener(UnityAction action) => throw new NotImplementedException("Typed EventVars must use typed events.");
        public override void RemoveListener(UnityAction action) => throw new NotImplementedException("Typed EventVars must use typed events.");

        public virtual void AddListener(UnityAction<OutT> action)
        {
            Debug.Assert(!RequireInstancing || IsActorInstance);

            AddListenerWithoutInvoke(action);

            if (invokeNewListeners) action.Invoke(Value);
        }

        public void AddListenerWithoutInvoke(UnityAction<OutT> action)
        {
            Debug.Assert(!RequireInstancing || IsActorInstance);

            Initialize();
            typedEvent.AddListener(action);
            runtimeListenerCount++;
        }

        public virtual void RemoveListener(UnityAction<OutT> action)
        {
            Initialize();
            typedEvent.RemoveListener(action);
            runtimeListenerCount--;
        }

        public void SetInitialValue(InT initialValue)
        {
            this.InitialValue = initialValue;
            if (isInitialized) ChangeCurrentValue(initialValue);
            else UntypedStoredValue = initialValue;
        }
        public abstract InT To(EventVarInstanceData bc);

        public override void SetInitialValue(EventVarInstanceData bc)
        {
            InT v = default;
            v = To(bc);
            SetInitialValue(v);
        }
    }

    public abstract class GenericSystemObjectEventVar<T> : EventVar<T>
    {
        public override T To(EventVarInstanceData bc) => (T)bc.SystemObjectValue;
#if UNITY_EDITOR
        protected override void SetInstanceConfigValue(T val, EventVarInstanceData config) => config.SystemObjectValue = val;
#endif
    }

    public abstract class GenericUnityObjectEventVar<T> : EventVar<T> where T : UnityEngine.Object
    {
        public override T To(EventVarInstanceData bc) => (T)bc.UnityObjectValue;
#if UNITY_EDITOR
        protected override void SetInstanceConfigValue(T val, EventVarInstanceData config) => config.UnityObjectValue = val;
#endif
    }
}