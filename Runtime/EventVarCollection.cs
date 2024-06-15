//alex@bardicbytes.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars
{
    [CreateAssetMenu(menuName = "BardicBytes/EventVar Collection")]
    public class EventVarCollection : ScriptableObject
    {

#if UNITY_EDITOR
        [ContextMenu("Export to File...")]
        public void ExportToFile()
        {
            var path = EditorUtility.SaveFilePanel("Save EventVarCollection to JSON file", "", name + ".json", "json");
            if (string.IsNullOrEmpty(path)) return;
            ExportToJson(path);
        }

        [ContextMenu("Import From File...")]
        public void ImportFromFile()
        {
            var path = EditorUtility.OpenFilePanel("Open a JSON file", "", "json");
            if (string.IsNullOrEmpty(path)) return;
            ImportFromJson(path);
        }
#endif

        // Serialized / Inspector Fields
        [SerializeField]
        private List<BaseEventVar> eventVars = default;

        [SerializeField]
        [HideInInspector]
        private List<int> guidHash = default;

        [field: SerializeField]
        public bool ExportPretty { get; set; } = false;

        [Header("Gameplay Save and Load")]
        [SerializeField]
        private string saveFolderName = "saves";
        [SerializeField]
        private bool useFolders = false;

        // accessors
        public T Get<T>(int index) where T : BaseEventVar => (T)eventVars[index];

        public BaseEventVar this[int index] => eventVars[index];
        public int Count => eventVars.Count;

        private void OnValidate()
        {
            bool valid = eventVars.Count == guidHash.Count;
            if (!valid) guidHash = new List<int>(new int[eventVars.Count]);

            for (int i = 0; i < eventVars.Count; i++)
            {
                if (eventVars[i] == null) break;

                int hash = eventVars[i].GUID.GetHashCode();

                if (guidHash[i] == 0 || hash != guidHash[i])
                {
                    guidHash[i] = hash;
                }
            }
        }

        /// <summary>
        /// updates the values of the eventVars without raising the events.
        /// </summary>
        /// <param name="data">serializable eventVar data</param>
        public void Import(SerializableEventVarCollection data)
        {
#if DEBUG
            if(Debug.isDebugBuild) Debug.Assert(data != null, "Import Error, data is null");
#endif
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

        [ContextMenu("Log Export")]
        public void LogExport() => Debug.Log(JsonUtility.ToJson(Export(), true));

        public void ExportToJson(string path) => File.WriteAllText(path,JsonUtility.ToJson(Export(), ExportPretty));

        /// <summary>
        /// updates the values of the eventVars without raising the events.
        /// </summary>
        /// <param name="path">the full path of the json file</param>
        public void ImportFromJson(string path) => Import(DeserializeJsonAtPath(path));

        /// <summary>
        /// loads and deserialized the json file at the path
        /// </summary>
        /// <param name="path">the full path of the json file</param>
        /// <returns>returns null if there was an error reading the file or deserializing the json</returns>
        private static SerializableEventVarCollection DeserializeJsonAtPath(string path)
        {
            var text = File.ReadAllText(path);

            try
            {
                var data = JsonUtility.FromJson<SerializableEventVarCollection>(text);
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public async Task SaveAsync(string filename)
        {
            var path = GetPath(filename);
            var json = JsonUtility.ToJson(Export(), ExportPretty);

            try
            {
                await File.WriteAllTextAsync(path, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save file at {path}: {e}");
            }
        }

        public void SaveDefault() => Save("default");
        public void LoadDefault() => Load("default");

        public void Save(string filename) => ExportToJson(GetPath(filename));
        public void Load(string filename) => ImportFromJson(GetPath(filename));

        private string GetPath(string filename)
        {
            var f = useFolders ? ($"{filename}/") : "";
            return $"{Application.persistentDataPath}/{saveFolderName}/{f}{filename}.json";
        }
    }
}