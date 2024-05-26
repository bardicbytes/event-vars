//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVar Collection")]
    public class EventVarCollection : ScriptableObject
    {
        // Serialized / Inspector Fields
        [SerializeField]
        private List<EventVar> eventVars = default;
        [SerializeField]
        [HideInInspector]
        private List<int> guidHash = default;

        // accessors
        public T Get<T>(int index) where T : EventVar => (T)eventVars[index];

        public EventVar this[int index] => eventVars[index];
        public int Count => eventVars.Count;

        private void OnValidate()
        {
            bool valid = eventVars.Count == guidHash.Count;
            if (!valid) guidHash = new List<int>(new int[eventVars.Count]);

            for (int i = 0; i < eventVars.Count; i++)
            {
                if(eventVars[i] == null) break;

                int hash = eventVars[i].GUID.GetHashCode();

                if (guidHash[i] == 0 || hash != guidHash[i])
                {
                    guidHash[i] = hash;
                }
            }
        }

        public void Import(SerializableEventVarCollection data)
        {
            ImportList(data.floatEventVars);
            ImportList(data.boolEventVars);
            ImportList(data.intEventVars);
            ImportList(data.stringEventVars);
            ImportList(data.vector3EventVars);
            ImportList(data.vector2IntEventVars);

            void ImportList<T>(List<SerializableEventVarData<T>> data)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    var a = data[i];
                    var aHash = a.guid.GetHashCode();
                    var eventVarIndex = guidHash.IndexOf(aHash);
                    eventVars[eventVarIndex].SetStoredValue(a.value);
                }
            }
        }

        public SerializableEventVarCollection Export() => new SerializableEventVarCollection(eventVars);

        [ContextMenu("Print Export")]
        public void PrintExport() => Debug.Log(JsonUtility.ToJson(Export(), true));
    }
}