//alex@bardicbytes.com

using UnityEditor;

namespace BardicBytes.EventVars.Editor
{
    [CustomEditor(typeof(EventVar), true), CanEditMultipleObjects]
    public class EventVarEditor : UnityEditor.Editor
    {
        private bool foldout = false;

        public override void OnInspectorGUI()
        {
            var targetListener = (EventVar)target;
            EditorHelper.DrawPropertiesByName(serializedObject, targetListener.EditorProperties);

            foldout = EditorGUILayout.Foldout(foldout, "Default Inspector", true);
            if (foldout) DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }
}