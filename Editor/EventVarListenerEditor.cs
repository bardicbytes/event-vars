using UnityEditor;

namespace BardicBytes.EventVars.Editor
{
    [CustomEditor(typeof(EventVarListener), true), CanEditMultipleObjects]
    public class EventVarListenerEditor : UnityEditor.Editor
    {
        private bool foldout = false;

        public override void OnInspectorGUI()
        {
            var targetListener = (EventVarListener)target;
            EditorHelper.DrawPropertiesByName(serializedObject, targetListener.EditorProperties);

            foldout = EditorGUILayout.Foldout(foldout, "Default Inspector", true);
            if (foldout) DrawDefaultInspector();
        }
    }
}