//alex@bardicbytes.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
        private List<string> assetGuids = default;

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
        public int Count => eventVars == null ? 0 : eventVars.Count;

        private void OnValidate()
        {
            if (eventVars == null) eventVars = new List<BaseEventVar>();
            if (assetGuids == null || assetGuids.Count != eventVars.Count)
            {
                var next = new List<string>(eventVars.Count);
                for (int i = 0; i < eventVars.Count; i++)
                    next.Add(i < (assetGuids == null ? 0 : assetGuids.Count) ? assetGuids[i] : null);
                assetGuids = next;
            }

            for (int i = 0; i < eventVars.Count; i++)
            {
                if (eventVars[i] == null) continue;

                var id = eventVars[i].GUID;
                if (string.IsNullOrEmpty(assetGuids[i]) || assetGuids[i] != id)
                    assetGuids[i] = id;
            }
        }

        /// <summary>
        /// updates the values of the eventVars without raising the events.
        /// </summary>
        /// <param name="data">serializable eventVar data</param>
        public void Import(SerializableEventVarCollection data)
        {
            if (data == null)
            {
                Debug.LogError("Import Error, data is null");
                return;
            }

            ImportList(data.floatEventVars);
            ImportList(data.boolEventVars);
            ImportList(data.intEventVars);
            ImportList(data.stringEventVars);
            ImportList(data.vector3EventVars);
            ImportList(data.vector2EventVars);
            ImportList(data.vector2IntEventVars);
            ImportList(data.quaternionEventVars);

            void ImportList<T>(List<SerializableEventVarData<T>> entries)
            {
                if (entries == null) return;
                for (int i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];
                    if (entry == null) continue;
                    var eventVarIndex = FindEventVarIndex(entry.guid);
                    if (eventVarIndex < 0)
                    {
                        Debug.LogWarning($"EventVarCollection '{name}' has no entry for GUID {entry.guid}.");
                        continue;
                    }
                    try
                    {
                        eventVars[eventVarIndex].SetStoredValue(entry.value);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(
                            $"EventVarCollection '{name}' failed to import GUID {entry.guid} ({eventVars[eventVarIndex]?.name}): {e.Message}",
                            this);
                    }
                }
            }
        }

        private int FindEventVarIndex(string guid)
        {
            if (string.IsNullOrEmpty(guid) || eventVars == null) return -1;

            if (assetGuids != null)
            {
                int cached = assetGuids.IndexOf(guid);
                if (cached >= 0 && cached < eventVars.Count && eventVars[cached] != null)
                    return cached;
            }

            for (int i = 0; i < eventVars.Count; i++)
            {
                if (eventVars[i] != null && eventVars[i].GUID == guid) return i;
            }

            return -1;
        }

        public SerializableEventVarCollection Export() => new SerializableEventVarCollection(eventVars ?? new List<BaseEventVar>());

        [ContextMenu("Log Export")]
        public void LogExport() => Debug.Log(JsonUtility.ToJson(Export(), true));

        public void ExportToJson(string path) => File.WriteAllText(path, JsonUtility.ToJson(Export(), ExportPretty));

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
            try
            {
                var text = File.ReadAllText(path);
                return JsonUtility.FromJson<SerializableEventVarCollection>(text);
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
            EnsureSaveDirectory(path);
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

        public void Save(string filename)
        {
            var path = GetPath(filename);
            EnsureSaveDirectory(path);
            ExportToJson(path);
        }

        public void Load(string filename)
        {
            var path = GetPath(filename);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"No EventVarCollection save file at {path}");
                return;
            }
            ImportFromJson(path);
        }

        private static void EnsureSaveDirectory(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
        }

        private string GetPath(string filename)
        {
            var f = useFolders ? ($"{filename}/") : "";
            return $"{Application.persistentDataPath}/{saveFolderName}/{f}{filename}.json";
        }
    }
}