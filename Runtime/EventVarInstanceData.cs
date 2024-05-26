//alex@bardicbytes.com

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        public override string ToString()
        {
            return $"EventVarInstanceData: {editorName}.";
        }

        [HideInInspector]
        [SerializeField]
        private string editorName = "unnamed";

        [field: SerializeField]
        public EventVar Source { get; protected set; }
        
        [field: SerializeField] public string StringValue { get; set; }
        [field: SerializeField] public int IntValue { get; set; }
        [field: SerializeField] public float FloatValue { get; set; }
        [field: SerializeField] public bool BoolValue { get; set; }

        [field: SerializeField] public Vector3 Vector3Value { get; set; }
        [field: SerializeField] public Vector2Int Vector2IntValue { get; set; }
        [field: SerializeField] public Vector2 Vector2Value { get; set; }

        // for the likes of Transform, Rigidbody, and Gameobject
        [field: SerializeField] public UnityEngine.Object UnityObjectValue { get; set; }

        //for any non-unity Object serializable class
        [field: SerializeField] public System.Object SystemObjectValue { get; set; }

        private EventVar eventVarClone;

        public EventVar EventVarInstance
        {
            get
            {
                Debug.Assert(Application.isPlaying, "runtime only!");
                return eventVarClone;
            }
            set => eventVarClone = value;
        }

        public EventVarInstanceData()
        {
            editorName = "NEW@" + System.DateTime.Now;
        }

        public void RuntimeInitialize(EventVarInstancer owner)
        {
            Debug.Assert(Source != null, "source was not set before runtime initialization of event var instance data "+owner.name, owner);

            eventVarClone = Source.GetCreateActorInstance<EventVar>(owner);
            EventVarInstance.SetInitialValue(this);
        }


#if UNITY_EDITOR

        public void RefreshEditorName() => editorName = string.Format("{0}", Source.name);
       
#endif
    }
}