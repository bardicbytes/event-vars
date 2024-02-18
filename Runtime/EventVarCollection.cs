//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVar Collection")]
    public class EventVarCollection : ScriptableObject
    {
        public struct SerializableData
        {
            public string guid;
            public object value;

            public SerializableData(EventVar ev)
            {
                this.guid = ev.GUID;
                this.value = ev.UntypedStoredValue;
            }
        }

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
            if (!valid) guidHash = new List<int>(eventVars.Count);

            for (int i = default; i < eventVars.Count; i++)
            {
                int hash = eventVars[i].GUID.GetHashCode();

                if (guidHash[i] == 0 || hash != guidHash[i])
                {
                    guidHash[i] = hash;
                }
            }
        }

        public void Import(SerializableData[] data)
        {
            if(Debug.isDebugBuild) Debug.Assert(data.Length == guidHash.Count);

            for (int i = 0; i < data.Length; i++)
            {
                eventVars[i].SetStoredValue(data[i]);
            }
        }

        public SerializableData[] Export()
        {
            SerializableData[] data = new SerializableData[eventVars.Count];
            for(int i = 0; i < eventVars.Count; i++)
            {
                data[i] = new SerializableData(eventVars[i]);
            }
            return data;
        }
    }
}