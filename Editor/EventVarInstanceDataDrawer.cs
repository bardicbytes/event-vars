//alex@bardicbytes.com

using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars.Editor
{
    [CustomPropertyDrawer(typeof(EventVarInstanceData), true)]
    public class EventVarInstanceDataDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty topProp, GUIContent label)
        {
            var srcProp = topProp.FindPropertyRelative(StringUtility.GetBackingFieldName("Source"));
            
            if(srcProp == null)
            {
                EditorGUI.LabelField(position, "srcProp is null");
                return;
            }

            EditorGUI.BeginProperty(position, label, topProp);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect r1;
            Rect r2;

            if (srcProp.objectReferenceValue == null)
            {
                r1 = new Rect(position.x, position.y, 150, position.height);
                r2 = new Rect(position.x + r1.width + 5, position.y, position.width - r1.width - 5, position.height);
                EditorGUI.LabelField(r1, "Select an EventVar");
            }
            else if(srcProp.objectReferenceValue is EventAsset ev)
            {
                r1 = new Rect(position.x, position.y, position.width - 150, position.height);
                r2 = new Rect(position.x + r1.width + 5, position.y, position.width - r1.width - 5, position.height);

                bool changed = PropField(r1, topProp);
                if(changed)
                {
                    EditorUtility.SetDirty(topProp.serializedObject.targetObject);
                }
            }
            else
            {
                r2 = position;
            }

            EditorGUI.PropertyField(r2, srcProp, new GUIContent(""));

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Draws a property field for just the right serialized property
        /// </summary>
        /// <param name="position">gui rect</param>
        /// <param name="dataProperty">event var instance data property</param>
        /// <returns>true if changed</returns>
        public bool PropField(Rect position, SerializedProperty dataProperty)
        {
            EventVarInstanceData data = dataProperty.boxedValue as EventVarInstanceData;
            string selector;
            string objectName;

            if (data.Source is IEventVar eventVar)
            {
                selector = eventVar.StoredValueType.FullName;
                objectName = eventVar.StoredValueType.Name;
            }
            else
            {
                return false;
            }

            bool changed = false;
            bool drawn = false;

            SerializedProperty foundProperty = null;

            if (selector == typeof(string).FullName && DrawPropField("StringValue", out foundProperty) && foundProperty != null) data.StringValue = foundProperty.stringValue;
            else if (selector == typeof(int).FullName && DrawPropField("IntValue", out foundProperty) && foundProperty != null) data.IntValue = foundProperty.intValue;
            else if (selector == typeof(bool).FullName && DrawPropField("BoolValue", out foundProperty) && foundProperty != null) data.BoolValue = foundProperty.boolValue;
            else if (selector == typeof(float).FullName && DrawPropField("FloatValue", out foundProperty) && foundProperty != null) data.FloatValue = foundProperty.floatValue;
            else if (selector == typeof(Vector3).FullName && DrawPropField("Vector3Value", out foundProperty) && foundProperty != null) data.Vector3Value = foundProperty.vector3Value;
            else if (selector == typeof(Vector2Int).FullName && DrawPropField("Vector2IntValue", out foundProperty) && foundProperty != null) data.Vector2IntValue = foundProperty.vector2IntValue;
            else if (selector == typeof(Quaternion).FullName && DrawPropField("QuaternionValue", out foundProperty) && foundProperty != null) data.QuaternionValue = foundProperty.quaternionValue;

            if (changed) return changed;

            if (selector == typeof(UnityEngine.Object).FullName)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(position, FindPropRel("UnityObjectValue"), true);
                if (EditorGUI.EndChangeCheck())
                {
                    changed = true;
                    foundProperty = FindPropRel("UnityObjectValue");
                    if (foundProperty != null) data.StringValue = foundProperty.stringValue;
                }
                return changed;
            }
            else if (selector == typeof(System.Object).FullName)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(position, FindPropRel("SystemObjectValue"), true);
                if (EditorGUI.EndChangeCheck())
                {
                    changed = true;
                    foundProperty = FindPropRel("SystemObjectValue");
                    if (foundProperty != null) data.StringValue = foundProperty.stringValue;
                }
                EditorGUI.EndChangeCheck();
                return changed;
            }

            if(!drawn) EditorGUI.LabelField(position, "No Instancing Available for " + objectName);

            return false;

            bool DrawPropField(string propName, out SerializedProperty property)
            {
                EditorGUI.BeginChangeCheck();
                var bp = FindPropRel(propName);
                if (bp != null) EditorGUI.PropertyField(position, bp, new GUIContent(""),true);
                else EditorGUI.LabelField(position, propName + " null. " + dataProperty.propertyPath);
                changed = true;
                var c = EditorGUI.EndChangeCheck();
                changed = c;

                property = bp;
                return c;
            }

            SerializedProperty FindPropRel(string propName)
            {
                drawn = true;
                var s = StringUtility.GetBackingFieldName(propName);
                SerializedProperty sp = dataProperty.Copy();
                if (sp.name == s) return sp;
                while (sp.Next(true))
                {
                    if (sp.name == s) return sp;
                }
                return null;
            }
        }
    }
}

