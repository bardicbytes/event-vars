using UnityEditor;

namespace BardicBytes.EventVars.Editor
{
    public static class EditorHelper
    {
        public static void DrawPropertiesByName(SerializedObject serializedObject, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                SerializedProperty property = serializedObject.FindProperty(propertyName);

                if (property != null)
                {
                    EditorGUILayout.PropertyField(property, includeChildren: true);
                }
                else
                {
                    EditorGUILayout.HelpBox($"Property '{propertyName}' not found.", MessageType.Warning);
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}