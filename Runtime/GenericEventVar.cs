// alex@bardicbytes.com

using System;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace BardicBytes.EventVars
{
    public abstract class GenericEventVar<InT, OutT, EvT> : EventVar where EvT : GenericEventVar<InT, OutT, EvT>
    {
        public static implicit operator OutT(GenericEventVar<InT, OutT, EvT> ev) => ev.Value;

        /// <summary>
        /// The Actor's instance Value, the source asset's Value, or this field's fallback value; in that order.
        /// </summary>
        [Serializable]
        public class Field : GenericEventVarField<InT, OutT, EvT> { }

        [Serializable]
        public class UnityEvent : UnityEvent<OutT> { }

        [SerializeField]
        protected UnityEvent typedEvent = default;
        [SerializeField]
        protected InT initialValue = default;

        [Space]
        [SerializeField]
        protected bool logValueChange = false;
        [field: SerializeField]
        public bool RequireInstancing { get; protected set; }

        [Space]
        [SerializeField]
        protected bool resetValueOnDatalessRaise = true;
        [SerializeField]
        protected bool abortRaiseForIdenticalData = false;
        [SerializeField]
        [Tooltip("Also prevents reset/dataless raises if current value matches initial value")]
        protected bool requireData = false;
        [SerializeField]
        protected bool autoLock = false;
        [SerializeField]
        protected bool invokeNewListeners = false;

        [SerializeField]
        [HideInInspector]
        protected bool raiseOnInit = false;

        protected bool isLocked = false;

        public InT InitialValue => initialValue;
        public bool IsLocked => isLocked;
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

        protected override void OnValidate()
        {
            if (StoredValue == null
                || (StoredValue != null && !StoredValue.Equals(initialValue))) UntypedStoredValue = initialValue;
            base.OnValidate();
        }

        protected override void OnEnable()
        {
            UntypedStoredValue = initialValue;
            isLocked = false;
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

            if (false && raiseOnInit) Raise(initialValue);
            else SetInitialValue(initialValue);
        }


        public override string ToString() => GetValueString();

        public virtual string GetValueString()
        {
            return StoredValue == null ? "null value" : StoredValue.ToString();
        }

        public void Lock() => isLocked = true;

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
                && StoredValue.Equals(initialValue)) return;
            if (resetValueOnDatalessRaise) ChangeCurrentValue(initialValue);
            if (requireData) return;
            this.Raise(InitialValue);
            base.Raise();
        }

        public virtual void RaisePassthrough(SimpleGenericEventVar<InT> typedEventVar)
        {
            if (typedEventVar == this) throw new System.Exception("No Recursive EventVar Raising!");
            Raise(typedEventVar.Value);
        }

        public virtual void Raise(InT data)
        {
            Initialize();
            Debug.Assert(!RequireInstancing || IsActorInstance);
            if (IsLocked && abortRaiseForIdenticalData && StoredValue.Equals(data)) Debug.LogWarning("This locked event var can't be raised with identical data. " + name);
            if (IsLocked && !StoredValue.Equals(data)) Debug.LogError("Locked event vars are read only and cannot be raised new data" + name);
            if (IsLocked || abortRaiseForIdenticalData && StoredValue.Equals(data)) return;
            if (autoLock) Lock();
            ChangeCurrentValue(data);
            typedEvent.Invoke(Value);
            base.Raise();
        }

        private void ChangeCurrentValue(InT data)
        {
            if (Debug.isDebugBuild && logValueChange && !data.Equals(StoredValue))
            {
                Debug.Log(name + " value changed. " + StoredValue + "->" + data.ToString());
            }

            UntypedStoredValue = data;
        }

        public virtual void AddListener(UnityAction<OutT> action)
        {
            Debug.Assert(!RequireInstancing || IsActorInstance);

            AddListenerWithoutInvoke(action);

            if (invokeNewListeners)
            {
                action.Invoke(Value);
            }
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
            this.initialValue = initialValue;
            if (isInitialized) ChangeCurrentValue(initialValue);
            else UntypedStoredValue = initialValue;
        }
        public abstract InT To(EventVars.EventVarInstanceData bc);

        public override void SetInitialValue(EventVars.EventVarInstanceData bc)
        {
            InT v = default;
            v = To(bc);
            SetInitialValue(v);
        }
#if UNITY_EDITOR

        public InT To(SerializedProperty prop) => To((prop.boxedValue as EventVarInstanceData));

        public override void SetInitValueOfInstanceConfig(SerializedProperty prop, EventVarInstanceData config)
        {
            Debug.Assert(config != null);
            //var c = config as EventVarInstanceConfig;
            //Debug.Assert(c != null);
            var val = To(prop);
            SetInitialvalueOfInstanceConfig(val, config);
        }

        protected abstract void SetInitialvalueOfInstanceConfig(InT val, EventVarInstanceData config);


        //public override void SetInitialValue(SerializedProperty prop)
        //{
        //    InT v = default;
        //    v = To(prop);
        //    SetInitialValue(v);
        //}

        public virtual InT PropField(Rect position, UnityEditor.SerializedProperty rawProp)
        {
            EditorGUI.LabelField(position, initialValue.ToString());
            return default;
        }
#endif
    }
}