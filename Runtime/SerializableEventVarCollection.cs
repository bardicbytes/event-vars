//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// Holds collections of serialized event variable data for different types.
    /// Supports centralized management of various event variable data types.
    /// </summary>
    [System.Serializable]
    public class SerializableEventVarCollection
    {
        // Collections of serializable event variables for basic data types
        public List<SerializableEventVarData<string>> stringEventVars;
        public List<SerializableEventVarData<int>> intEventVars;
        public List<SerializableEventVarData<bool>> boolEventVars;
        public List<SerializableEventVarData<float>> floatEventVars;

        // Collections of serializable event variables for spatial data types
        public List<SerializableEventVarData<Vector3>> vector3EventVars;
        public List<SerializableEventVarData<Vector2Int>> vector2IntEventVars;
        public List<SerializableEventVarData<Quaternion>> quaternionEventVars;

        public SerializableEventVarCollection() { }

        /// <summary>
        /// Constructor that initializes the collections based on provided event variables.
        /// </summary>
        /// <param name="eventVars">List of event variables to initialize from.</param>
        public SerializableEventVarCollection(List<BaseEventVar> eventVars)
        {
            // Initialize collections for different event variable data types
            stringEventVars = new List<SerializableEventVarData<string>>();
            intEventVars = new List<SerializableEventVarData<int>>();
            boolEventVars = new List<SerializableEventVarData<bool>>();
            floatEventVars = new List<SerializableEventVarData<float>>();
            vector3EventVars = new List<SerializableEventVarData<Vector3>>();
            vector2IntEventVars = new List<SerializableEventVarData<Vector2Int>>();
            quaternionEventVars = new List<SerializableEventVarData<Quaternion>>();

            // Populate the collections based on the input event variables
            for (int i = 0; i < eventVars.Count; i++)
            {
                var eventVar = eventVars[i];

                if (eventVar is IEventVarInput<string> evS)
                {
                    stringEventVars.Add(evS.CreateSerializableData());
                }
                else if (eventVar is IEventVarInput<int> evI)
                {
                    intEventVars.Add(evI.CreateSerializableData());
                }
                else if (eventVar is IEventVarInput<bool> evB)
                {
                    boolEventVars.Add(evB.CreateSerializableData());
                }
                else if (eventVar is IEventVarInput<float> evF)
                {
                    floatEventVars.Add(evF.CreateSerializableData());
                }
                else if (eventVar is IEventVarInput<Vector3> evV3)
                {
                    vector3EventVars.Add(evV3.CreateSerializableData());
                }
                else if (eventVar is IEventVarInput<Vector2Int> evV2)
                {
                    vector2IntEventVars.Add(evV2.CreateSerializableData());
                }
                else if (eventVar is IEventVarInput<Quaternion> evQ)
                {
                    quaternionEventVars.Add(evQ.CreateSerializableData());
                }
            }
        }
    }
}
