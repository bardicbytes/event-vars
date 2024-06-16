using UnityEditor;

namespace BardicBytes.EventVars.Editor
{
    [CustomEditor(typeof(EventAssetListener), true), CanEditMultipleObjects]
    public class EventVarListenerEditor : UnityEditor.Editor
    {
        private bool _foldout = false;

        public override void OnInspectorGUI()
        {
            var targetListener = (EventAssetListener)target;
            EditorHelper.DrawPropertiesByName(serializedObject, targetListener.EditorProperties);

            _foldout = EditorGUILayout.Foldout(_foldout, "Default Inspector", true);
            if (_foldout) DrawDefaultInspector();
        }
    }
}