using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            List<EventVar> refreshTargets = new List<EventVar>();

            for (int i = 0; i < importedAssets.Length; i++)
            {
                string assetPath = importedAssets[i];
                ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                if (asset is EventVar eventVar)
                {
                    refreshTargets.Add(eventVar);
                }
            }

            for(int i = 0;i < refreshTargets.Count; i++)
            {
                var eventVar = refreshTargets[i];
                eventVar.RefreshGUID();
                EditorUtility.SetDirty(eventVar);
                AssetDatabase.SaveAssets();
            }
        }
    }
}