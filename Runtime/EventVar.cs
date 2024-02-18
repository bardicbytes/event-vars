// alex@bardicbytes.com

using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/EventVar without Data")]
    public class EventVar : ScriptableObject
    {
        public virtual string[] EditorProperties => new string[] { "untypedEvent"};

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
        public virtual bool HasValue { get => false; }
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
        public virtual void SetStoredValue(object newStoredValue)
        {
            this.UntypedStoredValue = newStoredValue;
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

            if(Debug.isDebugBuild) Debug.Assert(action != null);
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
        public override string ToString() => name + ". " + UntypedStoredValue;
    }

    /// <summary>
    /// A convenience implementation for EventVars that evaluate to the same type
    /// </summary>
    /// <typeparam name="T">The Type that the EventVar stores and shares when raised.</typeparam>
    public abstract class EventVar<T> : EventVar<T, T> { }

    /// <summary>
    /// This generic EventVar is for when the Input type and Output type match.
    /// Most EventVars Evaluate to the same type.
    /// </summary>
    /// <typeparam name="InputType">Input Type that must extend the Output Type</typeparam>
    /// <typeparam name="OutputType">The Output Type</typeparam>
    public abstract class EventVar<InputType, OutputType> : EventVar<InputType, OutputType, EventVar<InputType, OutputType>> where InputType : OutputType
    {
        // This EventVar always Evaluates 
        public override OutputType Evaluate(InputType val) => val;
    }

    /// <summary>
    /// The base generic type of EventVar.
    /// </summary>
    /// <typeparam name="InputType">The Type required when raising the EventVar.</typeparam>
    /// <typeparam name="OutputType">The Type shared when the EventVar is raised.</typeparam>
    /// <typeparam name="EventVarType">The Type that is implementing this generic EventVar class.</typeparam>
    public abstract class EventVar<InputType, OutputType, EventVarType> : EventVar where EventVarType : EventVar<InputType, OutputType, EventVarType>
    {
        public override string[] EditorProperties => new string[] { StringFormatting.GetBackingFieldName("InitialValue"),"typedEvent", "requireData", "resetValueOnDatalessRaise","invokeNewListeners", "abortRaiseForIdenticalData" };

        public static implicit operator OutputType(EventVar<InputType, OutputType, EventVarType> ev) => ev.Value;

        /// <summary>
        /// The Actor's instance Value, the source asset's Value, or this field's fallback value; in that order.
        /// </summary>
        [Serializable]
        public class Field : EventVarField<InputType, OutputType, EventVarType> { } 

        [Serializable]
        public class UnityEvent : UnityEvent<OutputType> { }

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
        public InputType InitialValue { get; protected set; }
        public InputType TypedStoredValue { get; protected set; }

        public OutputType Value
        {
            get
            {
                if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsActorInstance, name + " " + (IsActorInstance ? "Is ActorInst" : "Not ActorInst"));
                return Evaluate(TypedStoredValue);
            }
        }


        public override bool HasValue { get => true; }
        public override Type StoredValueType => typeof(InputType);
        public override Type OutputValueType => typeof(OutputType);

        public override int TotalListeners => base.TotalListeners + typedEvent.GetPersistentEventCount();
        public override void SetStoredValue(object newStoredValue)
        {
            if(newStoredValue == null || newStoredValue == default)
            {
                TypedStoredValue = default;
            }
            else if(newStoredValue is InputType typedValue)
            {
                this.TypedStoredValue = typedValue;
            }
            else
            {
                throw new ArgumentException($"{name} {newStoredValue} {newStoredValue?.GetType()}");
            }

            base.SetStoredValue(newStoredValue);
        }
        protected abstract void SetInstanceConfigValue(InputType val, EventVarInstanceData config);

#if UNITY_EDITOR
        public InputType GetValueFromProperty(SerializedProperty prop) => GetTypedValue(prop.boxedValue as EventVarInstanceData);

        public override void SetInstanceConfigValue(SerializedProperty prop, EventVarInstanceData config)
        {
            if (Debug.isDebugBuild) Debug.Assert(config != null);
            var val = GetValueFromProperty(prop);
            SetInstanceConfigValue(val, config);
        }


        public virtual InputType PropField(Rect position, UnityEditor.SerializedProperty rawProp)
        {
            EditorGUI.LabelField(position, InitialValue.ToString());
            return default;
        }


        protected override void OnValidate()
        {
            if (TypedStoredValue == null
                || (TypedStoredValue != null
                && !TypedStoredValue.Equals(InitialValue)))
            {
                SetStoredValue(InitialValue);
            }

            base.OnValidate();
        }
#endif

        protected override void OnEnable()
        {
            SetStoredValue(InitialValue);

            isInitialized = false;

            if (typedEvent != null) typedEvent.RemoveAllListeners();

            base.OnEnable();
        }

        public virtual OutputType Evaluate() => Evaluate((InputType)UntypedStoredValue);

        public abstract OutputType Evaluate(InputType inValue);

        protected override void Initialize()
        {
            if (isInitialized || !Application.isPlaying) return;
            base.Initialize();

            SetInitialValue(InitialValue);
        }

        public override string ToString() => GetValueString();

        // seprate for extensibility purposes
        public virtual string GetValueString() => TypedStoredValue == null ? "null value" : TypedStoredValue.ToString();

        /// <summary>
        /// Clones the source EventVar asset, and puts it in a config
        /// </summary>
        /// <returns>A new EventvarInstanceConfig upcast as a BaseConfig</returns>
        public override EventVarInstanceData CreateInstanceConfig() => new EventVarInstanceData(this);

        public override void Raise()
        {
            Initialize();

            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsActorInstance);
            
            if (requireData
                && resetValueOnDatalessRaise
                && TypedStoredValue.Equals(InitialValue)) return;
            
            if (resetValueOnDatalessRaise) ChangeCurrentValue(InitialValue);
            
            if (requireData) return;

            this.Raise(InitialValue);
            base.Raise();
        }

        public virtual void Raise(InputType data)
        {
            Initialize();
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

        private void ChangeCurrentValue(InputType data)
        {
            if (Debug.isDebugBuild && debugLogValueChange && !data.Equals(TypedStoredValue)) Debug.Log($"{name} value changes. {TypedStoredValue}={data}");
            SetStoredValue(data);
        }

        public override void AddListener(UnityAction action) => throw new NotImplementedException("Typed EventVars must use typed events.");
        public override void RemoveListener(UnityAction action) => throw new NotImplementedException("Typed EventVars must use typed events.");

        public virtual void AddListener(UnityAction<OutputType> action)
        {
            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsActorInstance);

            AddListenerWithoutInvoke(action);

            if (invokeNewListeners) action.Invoke(Value);
        }

        public void AddListenerWithoutInvoke(UnityAction<OutputType> action)
        {
            if (Debug.isDebugBuild) Debug.Assert(!RequireInstancing || IsActorInstance);

            Initialize();
            typedEvent.AddListener(action);
            runtimeListenerCount++;
        }

        public virtual void RemoveListener(UnityAction<OutputType> action)
        {
            Initialize();
            typedEvent.RemoveListener(action);
            runtimeListenerCount--;
        }

        public void SetInitialValue(InputType initialValue)
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
        public abstract InputType GetTypedValue(EventVarInstanceData data);

        public override void SetInitialValue(EventVarInstanceData data) => SetInitialValue(GetTypedValue(data));
    }

    public abstract class GenericSystemObjectEventVar<T> : EventVar<T>
    {
        public override T GetTypedValue(EventVarInstanceData bc) => (T)bc.SystemObjectValue;
        protected override void SetInstanceConfigValue(T val, EventVarInstanceData config) => config.SystemObjectValue = val;
    }

    public abstract class GenericUnityObjectEventVar<T> : EventVar<T> where T : UnityEngine.Object
    {
        public override T GetTypedValue(EventVarInstanceData bc) => (T)bc.UnityObjectValue;
        protected override void SetInstanceConfigValue(T val, EventVarInstanceData config) => config.UnityObjectValue = val;
    }
}