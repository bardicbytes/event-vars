using UnityEngine;
using UnityEditor;

namespace BardicBytes.EventVars.Editor
{
    public class EventVarPostprocessor : AssetPostprocessor
    {
        // This method is called whenever an asset is created or imported
        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                // Load the asset at the path
                ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                // Check if the asset is of type EventVar
                if (asset is EventVar eventVar)
                {
                    // Call the RefreshGUID method on the EventVar instance
                    eventVar.RefreshGUID();
                    // Save the asset to ensure the new GUID is stored
                    EditorUtility.SetDirty(eventVar);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}