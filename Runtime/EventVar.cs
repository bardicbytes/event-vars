// alex@bardicbytes.com

using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    /// 
    /// This class serves as a base for various event variable types, enabling consistent 
    /// handling and triggering of events based on value changes. Derived classes can 
    /// specialize the type of data they handle, while inheriting the event-driven 
    /// infrastructure.
    /// </summary>
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/EventVar without Data")]
    public class EventVar : ScriptableObject
    {
        public abstract class BaseField
        {
            [SerializeField] protected EventVarInstancer _instancer;
            public virtual void Validate() => Debug.Assert(_instancer != null);
        }

        public virtual string[] EditorProperties => new string[] { "untypedEvent"};
        public virtual string[] AdvancedEditorProperties => new string[] {};

        [field: HideInInspector]
        [field: SerializeField]
        public string GUID { get; private set; } = Guid.NewGuid().ToString();

        /*
         * Serialized / Inspector Fields and AutoProperties
         */

        public virtual object UntypedStoredValue { get; protected set; }

        [SerializeField]
        protected UnityEvent untypedEvent = default;

        /*
         * Accessors
         */
        public virtual int TotalListeners => untypedEvent.GetPersistentEventCount() + runtimeListenerCount;
        public virtual bool ThisEventVarTypeHasValue { get => false; }
        public virtual Type StoredValueType => default;
        public virtual Type OutputValueType => default;

        /*
         * public auto properties
         */
        public EventVar CloneSource { get; protected set; } = null;
        public EventVarInstancer InstanceOwner { get; protected set; } = null;
        public bool IsActorInstance { get; protected set; } = false;

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
        private void Reset()
        {
            Reset_first_EventVar();
            if (string.IsNullOrEmpty(GUID) || GUID == name) RefreshGUID();
            Reset_last_EventVar();
        }

        private void OnValidate()
        {
            OnValidate_first_EventVar();
            lastRaiseTime = 0;
            runtimeListenerCount = 0;
            if (GUID == null) RefreshGUID();
            OnValidate_last_EventVar();
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
        public void RefreshGUID()
        {
            var p = AssetDatabase.GetAssetPath(this);
            GUID = AssetDatabase.AssetPathToGUID(p);
        }

        [MenuItem(itemName: "Bardic Bytes/EventVars/Refresh All GUIDs")]
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
#endif

        internal virtual void SetStoredValue(object newStoredValue)
        {
            this.UntypedStoredValue = newStoredValue;
        }

        protected virtual void OnEnable()
        {
            if (untypedEvent != null) untypedEvent.RemoveAllListeners();

            isInitialized = false;

            Initialize();
        }

        protected virtual void Initialize()
        {
            if (isInitialized || !Application.isPlaying) return;

            runtimeListenerCount = 0;

            lastRaiseTime = 0;
            isInitialized = true;
        }

        /*
         * public methods
         */


        /// <summary>
        /// Insantiates a clone of THIS.
        /// </summary>
        /// <typeparam name="T">The EventVar type</typeparam>
        /// <param name="owner">the associated instancer component</param>
        /// <returns></returns>
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
            if(Debug.isDebugBuild) Debug.Assert(action != null);
            untypedEvent.AddListener(action);
            runtimeListenerCount++;
        }

        public virtual void RemoveListener(UnityAction action)
        {
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


        public override string ToString() => name + ". " + UntypedStoredValue;
    }

    /// <summary>
    /// This "Self-Evaluating" type is a convenience implementation for EventVars that evaluate to the same type.
    /// </summary>
    /// <typeparam name="InputType">The Type that the EventVar stores and shares when raised.</typeparam>
    public abstract class EventVar<InputType> : 
        EventVar<InputType, InputType>, 
        IEventVarInput<InputType> { }

    /// <summary>
    /// This generic EventVar is for when the Input type and Output type match.
    /// Most EventVars Evaluate to the same type.
    /// </summary>
    /// <typeparam name="InputType">Input Type that must extend the Output Type</typeparam>
    /// <typeparam name="OutputType">The Output Type</typeparam>
    public abstract class EventVar<InputType, OutputType> : 
        EventVar<InputType, OutputType, EventVar<InputType, OutputType>>, 
        IEventVarInput<InputType> 
        where InputType : OutputType
    {
        // This EventVar always Evaluates 
        public override OutputType Evaluate(InputType val) => val;
    }

    /// <summary>
    /// The base generic type of EventVar. Inherit from this class to create an EventVar which has an output value different from its input value.
    /// When these events are raised, value of InputType is passed as an argument, but values of OutputType are emitted
    /// </summary>
    /// <typeparam name="TInput">The Type required when raising the EventVar.</typeparam>
    /// <typeparam name="TOutput">The Type shared when the EventVar is raised.</typeparam>
    /// <typeparam name="TEventVar">The Type that is implementing this generic EventVar class.</typeparam>
    public abstract class EventVar<TInput, TOutput, TEventVar> : 
        EventVar,
        IEventVarInput<TInput>
        where TEventVar : EventVar<TInput, TOutput, TEventVar>
    {
    
        public override string[] EditorProperties => new string[] {
            StringFormatting.GetBackingFieldName("InitialValue"),
            "typedEvent",
        };



        public override string[] AdvancedEditorProperties => new string[] {
            "requireData",
            "resetValueOnDatalessRaise",
            "invokeNewListeners",
            "abortRaiseForIdenticalData"
        };

        public static implicit operator TOutput(EventVar<TInput, TOutput, TEventVar> ev) => ev.Value;


        /// <summary>
        /// When used as a serialized member of an UnityEngine.Object like MonoBehaviours and ScriptableObject,
        /// allows a script to be configured to use an EventVar or a local value
        /// </summary>
        [Serializable]
        public class Field : BaseField
        {
            public static implicit operator TOutput(Field f) => f.Evaluate();

            [SerializeField] private TOutput _fallbackValue = default;
            [SerializeField] private TEventVar _initialSource = default;

            private TEventVar _currentSource = default;

            public TOutput Value => Evaluate();
            public TEventVar Source => _initialSource;

            private TOutput Evaluate()
            {
                if (_initialSource == null) return _fallbackValue;

                if (_currentSource == null) _currentSource = _initialSource;

                if (_instancer != null && _instancer.HasInstance(_currentSource))
                {
                    return _instancer.GetInstance(_currentSource).Value;
                }

                return _currentSource.Value;
            }

            /// <summary>
            /// sets the source override 
            /// </summary>
            /// <param name="newSource">if null, the source will be reset to the initial source whether or not the initial source is null</param>
            public void SetSourceOverride(TEventVar newSource)
            {
                var nextSource = newSource;
                if (nextSource == null) nextSource = _initialSource;
                _currentSource = nextSource;
            }
        }


        [Serializable]
        public class UnityEvent : UnityEvent<TOutput> { }

        [SerializeField]
        protected UnityEvent typedEvent = default;

        [Space]
        [SerializeField]
        protected bool debugLogValueChange = false;

        [field: SerializeField, Tooltip("TRUE indicates this EventVar must be instanced through an EventVarInstancer component and is not valid otherwise.")]
        public bool RequireInstancing { get; protected set; }

        [Space]
        [SerializeField, Tooltip("If TRUE, raising this EventVar with the parameterless override will reset the value of this EventVar to it's initial state.")]
        protected bool resetValueOnDatalessRaise = true;
        [SerializeField, Tooltip("If TRUE, the event will not raise if the data is identical.")]
        protected bool abortRaiseForIdenticalData = false;
        [SerializeField]
        [Tooltip("Also prevents reset/dataless raises if current value matches initial value")]
        protected bool requireData = false;
        [Tooltip("When TRUE, listener components are direclty invoked immediately when added.")]
        [SerializeField]
        protected bool invokeNewListeners = false;

        [field: SerializeField]
        public TInput InitialValue { get; protected set; }

        /// <summary>
        /// This is the current stored value of InputType
        /// </summary>
        public TInput TypedStoredValue { get; protected set; }

        /// <summary>
        /// This property evaluates TypedStoredValue and provides an OutputType
        /// </summary>
        public TOutput Value
        {
            get
            {
                if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsActorInstance, name + " " + (IsActorInstance ? "Is ActorInst" : "Not ActorInst"));
                return Evaluate(TypedStoredValue);
            }
        }

        public override bool ThisEventVarTypeHasValue { get => true; }
        public override Type StoredValueType => typeof(TInput);
        public override Type OutputValueType => typeof(TOutput);

        public override int TotalListeners => base.TotalListeners + typedEvent.GetPersistentEventCount();
        internal override void SetStoredValue(object newStoredValue)
        {
            if(newStoredValue == null || newStoredValue == default)
            {
                TypedStoredValue = default;
            }
            else if(newStoredValue is TInput typedValue)
            {
                this.TypedStoredValue = typedValue;
            }
            else
            {
                throw new ArgumentException($"{name} {newStoredValue} {newStoredValue?.GetType()}");
            }

            base.SetStoredValue(newStoredValue);
        }

#if UNITY_EDITOR

        protected override void OnValidate_first_EventVar()
        {
            if (TypedStoredValue == null
                || (TypedStoredValue != null
                && !TypedStoredValue.Equals(InitialValue)))
            {
                SetStoredValue(InitialValue);
            }
        }
#endif

        protected override void OnEnable()
        {
            SetStoredValue(InitialValue);

            isInitialized = false;

            if (typedEvent != null) typedEvent.RemoveAllListeners();

            base.OnEnable();
        }

        public virtual TOutput Evaluate() => Evaluate((TInput)UntypedStoredValue);

        public abstract TOutput Evaluate(TInput inValue);

        protected override void Initialize()
        {
            if (isInitialized || !Application.isPlaying) return;
            base.Initialize();

            SetInitialValue(InitialValue);
        }

        public override string ToString() => GetValueString();

        // seprate for extensibility purposes
        public virtual string GetValueString() => TypedStoredValue == null ? "null value" : TypedStoredValue.ToString();

        public override void Raise()
        {
            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsActorInstance);
            
            if (requireData
                && resetValueOnDatalessRaise
                && TypedStoredValue.Equals(InitialValue)) return;
            
            if (resetValueOnDatalessRaise) ChangeCurrentValue(InitialValue);
            
            if (requireData) return;

            this.Raise(InitialValue);
            base.Raise();
        }

        public virtual void Raise(TInput data)
        {
            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsActorInstance);

            if (abortRaiseForIdenticalData && TypedStoredValue.Equals(data)) return;

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

            base.Raise();
        }

        private void ChangeCurrentValue(TInput data)
        {
            if (Debug.isDebugBuild && debugLogValueChange && !data.Equals(TypedStoredValue)) Debug.Log($"{name} value changes. {TypedStoredValue}={data}");
            SetStoredValue(data);
        }

        public override void AddListener(UnityAction action) => throw new NotImplementedException("Typed EventVars must use typed events.");
        public override void RemoveListener(UnityAction action) => throw new NotImplementedException("Typed EventVars must use typed events.");

        public virtual void AddListener(UnityAction<TOutput> action)
        {
            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsActorInstance);

            AddListenerWithoutInvoke(action);

            if (invokeNewListeners) action.Invoke(Value);
        }

        public void AddListenerWithoutInvoke(UnityAction<TOutput> action)
        {
            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsActorInstance);

            typedEvent.AddListener(action);
            runtimeListenerCount++;
        }

        public virtual void RemoveListener(UnityAction<TOutput> action)
        {
            typedEvent.RemoveListener(action);
            runtimeListenerCount--;
        }

        public void SetInitialValue(TInput initialValue)
        {
            this.InitialValue = initialValue;
            if (isInitialized) ChangeCurrentValue(initialValue);
            else SetStoredValue(initialValue);
        }

        /// <summary>
        /// Abtract method for extracting the value from the argument.
        /// </summary>
        /// <param name="data">The EventVarInsanceData</param>
        /// <returns>The typed value stored in the EventVarInsanceData arg</returns>
        public abstract TInput GetTypedValue(EventVarInstanceData data);

        public override void SetInitialValue(EventVarInstanceData data) => SetInitialValue(GetTypedValue(data));

        public SerializableEventVarData<TInput> GetSerializableData() => new SerializableEventVarData<TInput>(this);
    }
}