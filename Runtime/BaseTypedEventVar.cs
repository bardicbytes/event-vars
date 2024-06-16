// alex@bardicbytes.com

using System;
using UnityEngine;
using UnityEngine.Events;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// The base generic type of EventVar. Inherit from this class to create an EventVar which has an output value different from its input value.
    /// When these events are raised, value of TInput is passed as an argument, but values of TOutput are emitted
    /// </summary>
    /// <typeparam name="TInput">The Type required when raising the EventVar.</typeparam>
    /// <typeparam name="TOutput">The Type shared when the EventVar is raised.</typeparam>
    /// <typeparam name="TEventVar">The Type that is implementing this generic EventVar class.</typeparam>
    public abstract partial class BaseTypedEventVar<TInput, TOutput, TEventVar> :
        BaseEventVar, IEventVarInput<TInput>, IEventVarOutput<TOutput>, IEventVar
        where TEventVar : BaseTypedEventVar<TInput, TOutput, TEventVar>
    {

#if UNITY_EDITOR

        /// <summary>
        /// This property provides the array of propertyNames that the custom editor will draw in the inspector window. 
        /// Override this property and return different member names to customize the editor.
        /// </summary>
        public override string[] EditorProperties => new string[] {
            StringUtility.GetBackingFieldName("InitialValue"),
            "_typedEvent",
        };

        /// <summary>
        /// Similar to EditorProperties, but these properties will be drawn within a foldout.
        /// </summary>
        public override string[] AdvancedEditorProperties => new string[] {
            "requireData",
            "resetValueOnDatalessRaise",
            "abortRaiseForIdenticalData"
        };

        protected override sealed void OnValidate()
        {
            base.OnValidate_first_EventVar();
            if (TypedStoredValue == null
                || (TypedStoredValue != null
                && !TypedStoredValue.Equals(InitialValue)))
            {
                SetStoredValue(InitialValue);
            }

            _lastRaiseTime = 0;
            _runtimeListenerCount = 0;
            if (string.IsNullOrEmpty(GUID)) RefreshGUID_EditorOnly();
            base.OnValidate_last_EventVar();
        }
#endif
        // this enables the implicit conversion of an EventVar to it's output type
        public static implicit operator TOutput(BaseTypedEventVar<TInput, TOutput, TEventVar> ev) => ev.Value;

        // defining this class as a nested type means
        // every extension of BasedTypeEventVar has a useable field.
        [Serializable] public class Field : EventVarField<TInput, TOutput, TEventVar> { }

        [SerializeField]
        protected UnityEvent<TOutput> _typedEvent = default;

        [Space]
        [SerializeField]
        protected bool _debugLogValueChange = false;

        [field: SerializeField, Tooltip("TRUE indicates this EventVar must be instanced through an EventVarInstancer component and is not valid otherwise.")]
        public bool RequireInstancing { get; protected set; }

        [Space]
        [SerializeField, Tooltip("If TRUE, raising this EventVar with the parameterless override will reset the value of this EventVar to it's initial state.")]
        protected bool _resetValueOnDatalessRaise = true;
        [SerializeField, Tooltip("If TRUE, the event will not raise if the data is identical.")]
        protected bool _abortRaiseForIdenticalData = false;
        [SerializeField]
        [Tooltip("Also prevents reset/dataless raises if current value matches initial value")]
        protected bool _requireData = false;

        [field: SerializeField]
        public TInput InitialValue { get; protected set; }

        /*
         * public properties
         */
        /// <summary>
        /// This is the current stored value of TInput
        /// </summary>
        public TInput TypedStoredValue { get; protected set; }


        ///// <summary>
        ///// The System.Type of the stored value.
        ///// </summary>
        public override Type StoredValueType => typeof(TInput);

        /// <summary>
        /// The sum total number of listeners registered to the following: the typed UnityEvent from EventVar, the untyped UnityEvent from EventAsset, and runtime listeners.
        /// </summary>
        public override int TotalListeners => base.TotalListeners + _typedEvent.GetPersistentEventCount();

        /// <summary>
        /// This property evaluates TypedStoredValue and provides an TOutput
        /// </summary>
        public TOutput Value
        {
            get
            {
#if DEBUG
                if (Debug.isDebugBuild)
                {
                    var s = (IsClone ? "Is ActorInst" : "Not ActorInst");
                    Debug.Assert(!RequireInstancing || IsClone, $"{name} {s}");
                }
#endif
                return Evaluate(TypedStoredValue);
            }
        }

        /*
         * public methods
         */

        /// <summary>
        /// Sets the stored value of the event var directly, without 
        /// </summary>
        /// <param name="newStoredValue">must be null, default, or an object of type TInput</param>
        /// <exception cref="ArgumentException">The argument was not the correct type.</exception>
        public override void SetStoredValue(object newStoredValue)
        {
            if (newStoredValue == null || newStoredValue == default)
            {
                TypedStoredValue = default;
            }
            else if (newStoredValue is TInput typedValue)
            {
                this.TypedStoredValue = typedValue;
            }
            else
            {
                throw new ArgumentException($"{name} {newStoredValue} {newStoredValue?.GetType()}");
            }
        }

        public override T GetCreateInstance<T>(EventVarInstancer instancer) => GetCreateInstance(instancer) as T;

        /// <summary>
        /// Insantiates a clone of THIS.
        /// </summary>
        /// <typeparam name="T">The EventVar type</typeparam>
        /// <param name="owner">the associated instancer component</param>
        /// <returns></returns>
        public TEventVar GetCreateInstance(EventVarInstancer owner)
        {
            var c = Instantiate(this);
            c.CloneSource = this;
            c.Owner = owner;
            c.IsClone = true;
            return c as TEventVar;
        }

        /// <summary>
        /// evaluates the stored input type value
        /// </summary>
        /// <returns>the evaluated output type value</returns>
        public virtual TOutput Evaluate() => Evaluate(TypedStoredValue);

        /// <summary>
        /// Interprets the inValue argument and returns an output value object
        /// </summary>
        /// <param name="inValue">an object of the Input type of the event var</param>
        /// <returns>an object of the output type of the event var</returns>
        public abstract TOutput Evaluate(TInput inValue);

        public override string ToString() => GetValueString();

        // seprate for extensibility purposes
        public virtual string GetValueString() => TypedStoredValue == null ? "null value" : TypedStoredValue.ToString();

        /// <summary>
        /// call this function to raise the event. The configuration of the event determines the outcome, and this function may be aborted.
        /// </summary>
        public override void Raise()
        {
#if DEBUG
            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsClone);
#endif

            if (_requireData
                && _resetValueOnDatalessRaise
                && TypedStoredValue.Equals(InitialValue)) return;

            if (_resetValueOnDatalessRaise) ChangeCurrentValue(InitialValue);

            if (_requireData) return;

            this.Raise(InitialValue);
            base.Raise();
        }

        /// <summary>
        /// This function will change the EventVar's value then raise the event.
        /// </summary>
        /// <param name="data">the new value for the event</param>
        public virtual void Raise(TInput data)
        {
#if DEBUG
            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsClone);
#endif
            if (_abortRaiseForIdenticalData && TypedStoredValue.Equals(data)) return;

            ChangeCurrentValue(data);

            try
            {
                _typedEvent.Invoke(Value);
            }
            catch (Exception ex)
            {
                if (Debug.isDebugBuild) Debug.LogError($"[f{Time.frameCount}] EventVar \"{name}\" Exception Thrown in UnityEvent-{Value.GetType().Name} ({_typedEvent}).");
                throw ex;
            }

            base.Raise();
        }


        /// <summary>
        /// adds the action to the event
        /// </summary>
        /// <param name="action">this action will be invoked when the typed event is raised</param>
        public virtual void AddListener(UnityAction<TOutput> action)
        {
            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsClone);

            _typedEvent.AddListener(action);
            _runtimeListenerCount++;
        }

        /// <summary>
        /// removes the action from the event
        /// </summary>
        /// <param name="action">this action will no longer be invoked</param>
        public virtual void RemoveListener(UnityAction<TOutput> action)
        {
            _typedEvent.RemoveListener(action);
            _runtimeListenerCount--;
        }

        /// <summary>
        /// Abtract method for extracting the value from the argument.
        /// </summary>
        /// <param name="data">The EventVarInsanceData</param>
        /// <returns>The typed value stored in the EventVarInsanceData arg</returns>
        public abstract TInput GetTypedValue(EventVarInstanceData data);

        public override void SetInitialValue(EventVarInstanceData data) => SetInitialValue(GetTypedValue(data));

        public SerializableEventVarData<TInput> CreateSerializableData() => new SerializableEventVarData<TInput>(this);

        // fix these:

        /// <summary>
        /// This function should not be used for typed eventvars. Use the AddListener override
        /// </summary>
        public override void AddListener(UnityAction action)
        {
            throw new NotImplementedException("Typed EventVars must use typed events.");
        }

        /// <summary>
        /// This function should not be used for typed eventvars. Use the RemoveListener override
        /// </summary>
        public override void RemoveListener(UnityAction action)
        {
            throw new NotImplementedException("Typed EventVars must use typed events.");
        }

        /*
         * private/protected methods
         */
        protected override void OnEnable()
        {
            SetStoredValue(InitialValue);

            _isInitialized = false;

            if (_typedEvent != null) _typedEvent.RemoveAllListeners();

            base.OnEnable();
        }

        protected override void Initialize()
        {
            if (_isInitialized || !Application.isPlaying) return;
            base.Initialize();
            SetInitialValue(InitialValue);
        }

        private void SetInitialValue(TInput initialValue)
        {
            this.InitialValue = initialValue;
            if (_isInitialized) ChangeCurrentValue(initialValue);
            else SetStoredValue(initialValue);
        }

        private void ChangeCurrentValue(TInput data)
        {
#if DEBUG
            bool shouldPrint = Debug.isDebugBuild && _debugLogValueChange && !data.Equals(TypedStoredValue);
            if (shouldPrint) Debug.Log($"{name} value changes. {TypedStoredValue}={data}");
#endif
            SetStoredValue(data);
        }

    }
}