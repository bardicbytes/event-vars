//alex@bardicbytes.com

using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars.Editor
{
    [CustomPropertyDrawer(typeof(EventVarInstanceData), true)]
    public class EventVarInstanceDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty topProp, GUIContent label)
        {
            var srcProp = topProp.FindPropertyRelative(StringUtility.GetBackingFieldName("Source"));

            if (srcProp == null)
            {
                EditorGUI.LabelField(position, "srcProp is null");
                return;
            }

            EditorGUI.BeginProperty(position, label, topProp);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (srcProp.objectReferenceValue == null)
            {
                var labelRect = new Rect(position.x, position.y, 150, position.height);
                var sourceRect = new Rect(position.x + labelRect.width + 5, position.y, position.width - labelRect.width - 5, position.height);
                EditorGUI.LabelField(labelRect, "Select an EventVar");
                DrawSourceField(topProp, srcProp, sourceRect);
            }
            else if (srcProp.objectReferenceValue is EventAsset)
            {
                var valueRect = new Rect(position.x, position.y, position.width - 150, position.height);
                var sourceRect = new Rect(position.x + valueRect.width + 5, position.y, position.width - valueRect.width - 5, position.height);

                PropField(valueRect, topProp);
                DrawSourceField(topProp, srcProp, sourceRect);
            }
            else
            {
                DrawSourceField(topProp, srcProp, position);
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private static void DrawSourceField(SerializedProperty topProp, SerializedProperty srcProp, Rect sourceRect)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(sourceRect, srcProp, GUIContent.none);
            if (!EditorGUI.EndChangeCheck()) return;

            topProp.serializedObject.ApplyModifiedProperties();
            if (topProp.boxedValue is not EventVarInstanceData data) return;

            data.SeedValuesFromSource();
            topProp.boxedValue = data;
            topProp.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(topProp.serializedObject.targetObject);
        }

        /// <summary>
        /// Draws a property field for the typed value that matches the source EventVar.
        /// </summary>
        public bool PropField(Rect position, SerializedProperty dataProperty)
        {
            if (dataProperty.boxedValue is not EventVarInstanceData data || data.Source is not IEventVar eventVar)
                return false;

            var storedType = eventVar.StoredValueType;

            if (storedType == typeof(string)) return DrawRelative(position, dataProperty, "StringValue");
            if (storedType == typeof(int)) return DrawRelative(position, dataProperty, "IntValue");
            if (storedType == typeof(bool)) return DrawRelative(position, dataProperty, "BoolValue");
            if (storedType == typeof(float)) return DrawRelative(position, dataProperty, "FloatValue");
            if (storedType == typeof(Vector3)) return DrawRelative(position, dataProperty, "Vector3Value");
            if (storedType == typeof(Vector2)) return DrawRelative(position, dataProperty, "Vector2Value");
            if (storedType == typeof(Vector2Int)) return DrawRelative(position, dataProperty, "Vector2IntValue");
            if (storedType == typeof(Quaternion)) return DrawRelative(position, dataProperty, "QuaternionValue");
            if (typeof(Object).IsAssignableFrom(storedType)) return DrawRelative(position, dataProperty, "UnityObjectValue");

            EditorGUI.LabelField(position, "No Instancing Available for " + storedType.Name);
            return false;
        }

        private static bool DrawRelative(Rect position, SerializedProperty dataProperty, string propName)
        {
            var property = FindBackingField(dataProperty, propName);
            if (property == null)
            {
                EditorGUI.LabelField(position, propName + " null. " + dataProperty.propertyPath);
                return false;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, GUIContent.none, true);
            return EditorGUI.EndChangeCheck();
        }

        private static SerializedProperty FindBackingField(SerializedProperty dataProperty, string propName)
        {
            var backingName = StringUtility.GetBackingFieldName(propName);
            var iterator = dataProperty.Copy();
            var end = dataProperty.GetEndProperty();

            if (iterator.name == backingName) return iterator;

            while (iterator.Next(true) && !SerializedProperty.EqualContents(iterator, end))
            {
                if (iterator.name == backingName) return iterator;
            }

            return null;
        }
    }
}
