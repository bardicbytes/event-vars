using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars.AudioEventVar.Editor
{
    [CustomPropertyDrawer(typeof(AudioEventVar))]
    public class AudioEventVarDrawer : PropertyDrawer
    {
        private SerializedObject targetSerializedObject;
        private bool isFoldoutOpen = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (isFoldoutOpen)
            {
                position.height = EditorGUIUtility.singleLineHeight / 2f;
            }

            string title = $"{(property.objectReferenceValue != null ? property.objectReferenceValue.name : "")}";

            // Foldout for the AudioEventVar
            isFoldoutOpen = EditorGUI.Foldout(position, isFoldoutOpen, title);

            if (isFoldoutOpen && property.objectReferenceValue != null)
            {
                // Indent for the nested inspector
                EditorGUI.indentLevel++;

                // Create or update SerializedObject
                if (targetSerializedObject == null || targetSerializedObject.targetObject != property.objectReferenceValue)
                {
                    targetSerializedObject = new SerializedObject(property.objectReferenceValue);
                }
                targetSerializedObject.Update();

                // Calculate rect for the inspector (excluding foldout height)
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                //// Draw the full inspector for the ScriptableObject
                //var propertyField = targetSerializedObject.GetIterator();
                //propertyField.NextVisible(true); // Skip script field
                //while (propertyField.NextVisible(false))
                //{
                //    position.height = EditorGUI.GetPropertyHeight(propertyField, true);
                //    EditorGUI.PropertyField(position, propertyField, true);
                //    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                //}

                var propertyField = targetSerializedObject.FindProperty(StringUtility.GetBackingFieldName("InitialValue"));

                //position.height = EditorGUIUtility.singleLineHeight * 5;
                EditorGUI.PropertyField(position, propertyField, true);
                //position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                targetSerializedObject.ApplyModifiedProperties();
                EditorGUI.indentLevel--;
            }
            else
            {
                position.x += 15f;
                position.width -= 15f;

                // Draw the object field if not folded out
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, property, GUIContent.none);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;
            if (isFoldoutOpen && property.objectReferenceValue != null)
            {
                if (targetSerializedObject == null || targetSerializedObject.targetObject != property.objectReferenceValue)
                {
                    targetSerializedObject = new SerializedObject(property.objectReferenceValue);
                }
                targetSerializedObject.Update();

                var propertyField = targetSerializedObject.GetIterator();
                propertyField.NextVisible(true); // Skip script field
                while (propertyField.NextVisible(false))
                {
                    height += EditorGUI.GetPropertyHeight(propertyField, true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            return height;
        }
    }

}