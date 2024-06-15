//alex@bardicbytes.com

using UnityEditor;

namespace BardicBytes.EventVars.Editor
{
    [CustomEditor(typeof(EventAsset), true), CanEditMultipleObjects]
    public class EventVarEditor : UnityEditor.Editor
    {
        private bool showDefaultInspector = false;
        private bool showAdvancedOptions = false;

        public override void OnInspectorGUI()
        {
            var targetListener = (EventAsset)target;
            EditorHelper.DrawPropertiesByName(serializedObject, targetListener.EditorProperties);

            showAdvancedOptions = EditorGUILayout.Foldout(showAdvancedOptions, "Advanced Options", true);
            if (showAdvancedOptions)
            {
                EditorHelper.DrawPropertiesByName(serializedObject, targetListener.AdvancedEditorProperties);
            }
            EditorGUILayout.Space(25);
            
            showDefaultInspector = EditorGUILayout.Foldout(showDefaultInspector, "Default Inspector", true);
            if (showDefaultInspector) DrawDefaultInspector();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}