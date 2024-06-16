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

        public void RefreshEditorName() => _editorName = string.Format("{0}", Source.name);

        public EventVarInstanceData() => _editorName = "NEW@" + System.DateTime.Now;
#endif

        public override string ToString() => $"EventVarInstanceData: {_editorName}.";


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

        //for any non-unity Object serializable class
        [field: SerializeField] public System.Object SystemObjectValue { get; set; }
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