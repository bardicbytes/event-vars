//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.EventVars
{
    /// <summary>
    /// SerializableEventVarCollection class
    /// Holds collections of serialized event variable data for different types.
    /// Supports centralized management of various event variable data types.
    /// </summary>
    [System.Serializable]
    public class SerializableEventVarCollection
    {
        // Collections of serializable event variables for basic data types
        public List<SerializableEventVarData<string>> stringEventVars; // Event variables for string type
        public List<SerializableEventVarData<int>> intEventVars;       // Event variables for int type
        public List<SerializableEventVarData<bool>> boolEventVars;     // Event variables for bool type
        public List<SerializableEventVarData<float>> floatEventVars;   // Event variables for float type

        // Collections of serializable event variables for spatial data types
        public List<SerializableEventVarData<Vector3>> vector3EventVars;       // Event variables for 3D spatial data
        public List<SerializableEventVarData<Vector2Int>> vector2IntEventVars; // Event variables for 2D pixel data

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

            // Populate the collections based on the input event variables
            for (int i = 0; i < eventVars.Count; i++)
            {
                var eventVar = eventVars[i];

                // Add event variables based on their type
                if (eventVar is IEventVarInput<string> evS)
                {
                    stringEventVars.Add(evS.GetSerializableData());
                }
                else if (eventVar is IEventVarInput<int> evI)
                {
                    intEventVars.Add(evI.GetSerializableData());
                }
                else if (eventVar is IEventVarInput<bool> evB)
                {
                    boolEventVars.Add(evB.GetSerializableData());
                }
                else if (eventVar is IEventVarInput<float> evF)
                {
                    floatEventVars.Add(evF.GetSerializableData());
                }
                else if (eventVar is IEventVarInput<Vector3> evV3)
                {
                    vector3EventVars.Add(evV3.GetSerializableData());
                }
                else if (eventVar is IEventVarInput<Vector2Int> evV2)
                {
                    vector2IntEventVars.Add(evV2.GetSerializableData());
                }
            }
        }
    }
}
