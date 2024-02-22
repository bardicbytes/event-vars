//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.EventVars
{
    [System.Serializable]
    public class SerializableEventVarCollection
    {
        //the basics
        public List<SerializableEventVarData<string>> stringEventVars;
        public List<SerializableEventVarData<int>> intEventVars;
        public List<SerializableEventVarData<bool>> boolEventVars;
        public List<SerializableEventVarData<float>> floatEventVars;

        //for 3D spatial
        public List<SerializableEventVarData<Vector3>> vector3EventVars;
        //for 2D pixels
        public List<SerializableEventVarData<Vector2Int>> vector2IntEventVars;

        public SerializableEventVarCollection() { }
        public SerializableEventVarCollection(List<EventVar> eventVars)
        {
            stringEventVars = new List<SerializableEventVarData<string>>();
            intEventVars = new List<SerializableEventVarData<int>>();
            boolEventVars = new List<SerializableEventVarData<bool>>();
            floatEventVars = new List<SerializableEventVarData<float>>();
            vector3EventVars = new List<SerializableEventVarData<Vector3>>();
            vector2IntEventVars = new List<SerializableEventVarData<Vector2Int>>();

            for (int i = 0; i < eventVars.Count; i++)
            {
                var eventVar = eventVars[i];
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