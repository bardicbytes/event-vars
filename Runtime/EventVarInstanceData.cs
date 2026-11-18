//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// The EventVarInstanceData class is responsible for storing data associated with instances of event variables.
    /// This includes both the event variable's metadata and its stored value.
    /// 
    /// The class provides:
    /// - A means to serialize and deserialize event variable data for persistence.
    /// - Facilities to clone and initialize event variables during runtime.
    /// - Type-specific properties for commonly used data types, ensuring easy access and manipulation.
    /// 
    /// This class helps to encapsulate the logic and data associated with specific instances of event variables,
    /// providing a standardized way to handle them across the application.
    /// </summary>
    [System.Serializable]
    public class EventVarInstanceData
    {
#if UNITY_EDITOR
        [HideInInspector]
        [SerializeField]
        private string _editorName = "unnamed";

        public void RefreshEditorName() =>
            _editorName = Source != null ? Source.name : "unnamed";

        public EventVarInstanceData() => _editorName = "NEW@" + System.DateTime.Now;

        /// <summary>
        /// Copies the source asset's InitialValue into the matching typed fields.
        /// Call when Source is assigned so clones do not silently fall back to type defaults.
        /// </summary>
        public void SeedValuesFromSource()
        {
            if (Source == null) return;

            switch (Source)
            {
                case FloatEventVar f:
                    FloatValue = f.InitialValue;
                    break;
                case IntEventVar i:
                    IntValue = i.InitialValue;
                    break;
                case BoolEventVar b:
                    BoolValue = b.InitialValue;
                    break;
                case StringEventVar s:
                    StringValue = s.InitialValue;
                    break;
                case Vector3EventVar v3:
                    Vector3Value = v3.InitialValue;
                    break;
                case Vector2EventVar v2:
                    Vector2Value = v2.InitialValue;
                    break;
                case Vector2IntEventVar v2i:
                    Vector2IntValue = v2i.InitialValue;
                    break;
                case QuaternionEventVar q:
                    QuaternionValue = q.InitialValue;
                    break;
                case TransformEventVar t:
                    UnityObjectValue = t.InitialValue;
                    break;
                case GameObjectEventVar go:
                    UnityObjectValue = go.InitialValue;
                    break;
                case RigidbodyEventVar rb:
                    UnityObjectValue = rb.InitialValue;
                    break;
            }

            RefreshEditorName();
        }
#endif

        public override string ToString()
        {
#if UNITY_EDITOR
            return $"EventVarInstanceData: {_editorName}.";
#else
            return Source != null ? Source.name : "EventVarInstanceData";
#endif
        }

        public void DestroyClone()
        {
            if (_eventVarClone == null) return;
            if (Application.isPlaying) Object.Destroy(_eventVarClone);
            else Object.DestroyImmediate(_eventVarClone);
            _eventVarClone = null;
        }


        #region value auto properties
        [field: SerializeField] public string StringValue { get; set; }
        [field: SerializeField] public int IntValue { get; set; }
        [field: SerializeField] public float FloatValue { get; set; }
        [field: SerializeField] public bool BoolValue { get; set; }

        [field: SerializeField] public Vector3 Vector3Value { get; set; }
        [field: SerializeField] public Vector2Int Vector2IntValue { get; set; }
        [field: SerializeField] public Vector2 Vector2Value { get; set; }
        [field: SerializeField] public Quaternion QuaternionValue { get; set; }

        // for the likes of Transform, Rigidbody, and Gameobject
        [field: SerializeField] public UnityEngine.Object UnityObjectValue { get; set; }
        #endregion


        /// <summary>
        /// This is the source asset from which _eventVarClone was cloned
        /// </summary>
        [field: SerializeField]
        public BaseEventVar Source { get; protected set; }

        private BaseEventVar _eventVarClone;

        /// <summary>
        /// This is the clone of Source.
        /// </summary>
        public BaseEventVar EventVarInstance
        {
            get
            {
                Debug.Assert(Application.isPlaying, "runtime only!");
                return _eventVarClone;
            }
            set => _eventVarClone = value;
        }

        /// <summary>
        /// creates a clone of the source asset.
        /// called by EventVarInstancer
        /// </summary>
        /// <param name="owner"></param>
        public void RuntimeInitialize(EventVarInstancer owner)
        {
            Debug.Assert(Source != null, "source was not set before runtime initialization of event var instance data " + owner.name, owner);

            _eventVarClone = Source.GetCreateInstance<BaseEventVar>(owner);
            EventVarInstance.SetInitialValue(this);
        }
    }
}