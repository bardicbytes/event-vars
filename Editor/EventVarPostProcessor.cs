using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars.Editor
{
    // before this class, copied EventVars had cached GUID issues
    /// <summary>
    /// This class makes sure that when a new eventVar is created or even copied from an existing asset, the GUID is refreshed.
    /// </summary>
    public class EventVarPostprocessor : AssetPostprocessor
    {
        // This method is called whenever an asset is created or imported
        static void OnPostprocessAllAssets(
            string[] importedAssetPaths,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            List<EventAsset> refreshTargets = new List<EventAsset>();

            for (int i = 0; i < importedAssetPaths.Length; i++)
            {
                ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(importedAssetPaths[i]);

                var guid = AssetDatabase.AssetPathToGUID(importedAssetPaths[i]);

                // if we're importing an EventAsset, make sure the guid is correct
                if (asset is EventAsset eventVar && eventVar.GUID != guid)
                {
                    refreshTargets.Add(eventVar);
                }
            }

            for (int i = 0; i < refreshTargets.Count; i++)
            {
                var eventVar = refreshTargets[i];
                eventVar.RefreshGUID_EditorOnly();
                EditorUtility.SetDirty(eventVar);
                AssetDatabase.SaveAssets();
            }
        }
    }
}