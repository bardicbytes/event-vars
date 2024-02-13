//alex@bardicbytes.com

using UnityEditor;
using BardicBytes.EventVars;

namespace BardicBytes.EventVarsEditor
{
    [CustomEditor(typeof(EventVar), true), CanEditMultipleObjects]
    public class EventVarEditor : Editor
    {
        private bool foldout = false;

        public override void OnInspectorGUI()
        {
            var targetListener = (EventVar)target;
            EditorHelper.DrawPropertiesByName(serializedObject, targetListener.EditorProperties);

            foldout = EditorGUILayout.Foldout(foldout, "Default Inspector", true);
            if (foldout) DrawDefaultInspector();
        }
    }
}