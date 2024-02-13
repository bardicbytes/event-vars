//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.EventVars
{
    public class EventVarInstancer : MonoBehaviour
    {
        [SerializeField]
        private List<EventVar> eventVars;

        [SerializeField]
        [SerializeReference]
        private EventVarInstanceData[] eventVarInstances;

        private Dictionary<EventVar, int> evInstanceLookup = default;

        private bool isInitialized = false;

#if UNITY_EDITOR

        [HideInInspector]
        [SerializeField]
        private int[] evHashCache;
        protected void OnValidate()
        {

            bool refreshEvCache = false;

            if (eventVars != null &&
                (evHashCache == null || evHashCache.Length != eventVars.Count
                || eventVarInstances == null || eventVarInstances.Length != eventVars.Count))
            {
                refreshEvCache = true;
                List<EventVar> found = new List<EventVar>();
                for (int i = 0; i < eventVars.Count; i++)
                {
                    refreshEvCache &= eventVars[i] != null;
                    if (found.Contains(eventVars[i]) || eventVars[i] == null)
                    {
                        refreshEvCache = false;
                        break;
                    }
                    found.Add(eventVars[i]);
                }
                //null or miscount
            }

            if (!refreshEvCache && eventVars != null && eventVars.Count > 0)
            {
                for (int i = 0; i < eventVars.Count; i++)
                {
                    if (eventVars[i] == null) continue;
                    if (eventVarInstances[i] == null)
                    {
                        refreshEvCache = true;
                        break;
                    }
                    //compare each event var to the cache
                    if (evHashCache[i] != eventVars[i].GUID.GetHashCode())
                    {
                        refreshEvCache = true;
                        break;
                    }
                }
            }

            if (refreshEvCache && eventVars != null && eventVars.Count > 0)
            {
                var instBackup = eventVarInstances;
                var cacheBackup = evHashCache;
                eventVarInstances = new EventVarInstanceData[eventVars.Count];
                evHashCache = new int[eventVars.Count];
                for (int i = 0; i < eventVars.Count; i++)
                {
                    if (eventVars[i] == null)
                    {
                        evHashCache[i] = 0;
                        continue;
                    }
                    if (instBackup == null || cacheBackup == null || instBackup.Length != cacheBackup.Length)
                    {
                        eventVarInstances[i] = eventVars[i].CreateInstanceConfig();
                    }
                    else
                    {
                        var found = Restorebackup(eventVars[i], instBackup, cacheBackup);
                        eventVarInstances[i] = found == null ? eventVars[i].CreateInstanceConfig() : found;
                    }
                    eventVarInstances[i].RefreshEditorName();
                    evHashCache[i] = eventVars[i].GUID.GetHashCode();
                }
            }

            EventVarInstanceData Restorebackup(EventVar src, EventVarInstanceData[] instBackup, int[] cacheBackup)
            {
                Debug.Assert(instBackup != null);
                Debug.Assert(cacheBackup != null);
                Debug.Assert(instBackup.Length == cacheBackup.Length);

                EventVarInstanceData c = null;
                for (int i = 0; i < instBackup.Length; i++)
                {
                    if (src.GUID.GetHashCode() == cacheBackup[i]) c = instBackup[i];
                }
                return c;
            }
        }

        [ContextMenu("RefreshEditorNames()")]
        private void RefreshEditorNames()
        {
            for (int i = 0; i < eventVarInstances.Length; i++)
                eventVarInstances[i].RefreshEditorName();
        }
#endif

        public void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (isInitialized && Application.isPlaying && evInstanceLookup != null)
            {
                return;
            }
            evInstanceLookup = new Dictionary<EventVar, int>();
            for (int i = 0; i < eventVars.Count; i++)
            {
                if (eventVars[i] == null) continue;
                eventVarInstances[i].RuntimeInitialize(this);
                evInstanceLookup.Add(eventVars[i], i);
            }
            isInitialized = true;
        }

        public T GetInstance<T>(T eventVarAssetRef) where T : EventVar => GetInstance(eventVarAssetRef as EventVar) as T;

        public EventVar GetInstance(EventVar eventVarAssetRef)
        {
            EventVar ev = null;
            if (!HasInstance(eventVarAssetRef))
            {
                Debug.LogWarning("no instance found, Check with HasInstance first.");
                return ev;
            }

            if (Application.isPlaying)
            {
                Initialize();
                var index = evInstanceLookup[eventVarAssetRef];
                ev = eventVarInstances[index].ActorInstance as EventVar;
            }
            else
            {
                Debug.LogWarning("GetIsntance at Editor time returns null");
                ev = null;
            }
            Debug.Assert(ev != null, "has instance, but no instance found?, but that's not cool. Initialized? " + isInitialized);
            return ev;
        }

        /// <summary>
        /// for editor time only. use GetInstance at runtime
        /// </summary>
        /// <param name="ev">the ev asset</param>
        /// <returns>the instance data for that asset</returns>
        public EventVarInstanceData FindInstanceData(EventVar ev)
        {
            EventVarInstanceData e = null;
            for (int i = 0; i < eventVarInstances.Length; i++)
            {
                e = eventVarInstances[i];
                if (e.Src != ev) continue;
            }
            return e;
        }

        public bool HasInstance(EventVar ev)
        {
            if (!Application.isPlaying)
            {
                return eventVars.Contains(ev);
            }
            if (evInstanceLookup == null) Initialize();
            return evInstanceLookup.ContainsKey(ev);
        }
    }
}