//alex@bardicbytes.com

using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars.Editor
{
    [CustomPropertyDrawer(typeof(EventVarField), true)]
    public class EventVarFieldDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty evFieldProperty, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, evFieldProperty);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            //EditorGUI.indentLevel = 0;

            var r = new Rect(position.x, position.y, 200, position.height);
            SerializedProperty evProp = evFieldProperty.FindPropertyRelative("srcEV");

            if (evProp.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(r, evFieldProperty.FindPropertyRelative("fallbackValue"), GUIContent.none);
                r = new Rect(position.x + r.width + 5, position.y, position.width - r.width - 5, position.height);
            }
            else
            {
                r = position;
            }
            EditorGUI.PropertyField(r, evProp, GUIContent.none);
            EditorGUI.indentLevel = indent;

            SerializedProperty instancerProp = evFieldProperty.FindPropertyRelative("instancer");
            var hostObject = evFieldProperty.serializedObject.targetObject as MonoBehaviour;
            var instancer = hostObject.GetComponent<EventVarInstancer>();
            bool srcEventVarIsSet = evProp.objectReferenceValue != null;
            
            if(instancer == null && srcEventVarIsSet)
            {
                instancer = hostObject.gameObject.AddComponent<EventVarInstancer>();
            }
            
            if (instancer != instancerProp.objectReferenceValue)
            {
                //var instancer = evFieldProperty.serializedObject.targetObject as EventVarInstancer;
                instancerProp.objectReferenceValue = instancer;
                //if (instancer != null) EditorUtility.SetDirty(instancer);
                evFieldProperty.serializedObject.ApplyModifiedProperties();
                instancerProp.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndProperty();
        }
    }
}