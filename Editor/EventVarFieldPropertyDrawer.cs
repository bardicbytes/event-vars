//alex@bardicbytes.com

using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars.Editor
{
    /// <summary>
    /// This class is responsible for drawing EventVar.Field's inspector property.
    /// </summary>
    [CustomPropertyDrawer(typeof(BaseEventVarField), true)]
    public class EventVarFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty evFieldProperty, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, evFieldProperty);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var rect = new Rect(position.x, position.y, 200, position.height);
            SerializedProperty evProp = evFieldProperty.FindPropertyRelative("_initialSource");

            if (evProp.objectReferenceValue == null)
            {
                // _fallbackValue only exists in EventVar.Field, not BaseField.
                EditorGUI.PropertyField(rect, evFieldProperty.FindPropertyRelative("_fallbackValue"), GUIContent.none);
                rect = new Rect(position.x + rect.width + 5, position.y, position.width - rect.width - 5, position.height);
            }
            else
            {
                rect = position;
            }

            EditorGUI.PropertyField(rect, evProp, GUIContent.none);

            SerializedProperty instancerProp = evFieldProperty.FindPropertyRelative("_instancer");
            var hostObject = evFieldProperty.serializedObject.targetObject as MonoBehaviour;

            if (hostObject != null &&
                instancerProp.objectReferenceValue == null &&
                evProp.objectReferenceValue != null)
            {
                var siblingInstancer = hostObject.GetComponent<EventVarInstancer>();
                if (siblingInstancer != null)
                {
                    instancerProp.objectReferenceValue = siblingInstancer;
                    evFieldProperty.serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
