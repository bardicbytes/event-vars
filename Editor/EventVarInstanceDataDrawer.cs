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
            var srcProp = topProp.FindPropertyRelative(StringFormatting.GetBackingFieldName("Source"));
            
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
            else if(srcProp.objectReferenceValue is EventVar ev)
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
            
            if (!data.Source.ThisEventVarTypeHasValue) return false;

            var selector = data.Source.StoredValueType.FullName;

            bool changed = false;
            bool drawn = false;

            if (selector == typeof(string).FullName && DrawPropField("StringValue"))
            {
                var p = FindPropRel("StringValue");
                if (p != null) data.StringValue = p.stringValue;
            }
            else if (selector == typeof(int).FullName && DrawPropField("IntValue"))
            {
                var p = FindPropRel("IntValue");
                if (p != null) data.IntValue = p.intValue;
            }
            else if (selector == typeof(bool).FullName && DrawPropField("BoolValue"))
            {
                var p = FindPropRel("BoolValue");
                if (p != null) data.BoolValue = p.boolValue;
            }
            else if (selector == typeof(float).FullName && DrawPropField("FloatValue"))
            {
                var p = FindPropRel("FloatValue");
                if (p != null) data.FloatValue = p.floatValue;
            }
            else if (selector == typeof(Vector3).FullName && DrawPropField("Vector3Value"))
            {
                var p = FindPropRel("Vector3Value");
                if (p != null) data.Vector3Value = p.vector3Value;
            }
            else if (selector == typeof(Vector2Int).FullName && DrawPropField("Vector2IntValue"))
            {
                var p = FindPropRel("Vector2IntValue");
                if (p != null) data.Vector2IntValue = p.vector2IntValue;
            }

            if (changed) return changed;

            if (selector == typeof(UnityEngine.Object).FullName)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(position, FindPropRel("UnityObjectValue"), true);
                if (EditorGUI.EndChangeCheck())
                {
                    changed = true;
                    var p = FindPropRel("UnityObjectValue");
                    if (p != null) data.StringValue = p.stringValue;
                }
                return changed;
            }

            if (selector == typeof(System.Object).FullName)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(position, FindPropRel("SystemObjectValue"), true);
                if (EditorGUI.EndChangeCheck())
                {
                    changed = true;
                    var p = FindPropRel("SystemObjectValue");
                    if (p != null) data.StringValue = p.stringValue;
                }
                EditorGUI.EndChangeCheck();
                return changed;
            }

            if(!drawn) EditorGUI.LabelField(position, "No Instancing Available for " + data.Source.StoredValueType.Name);

            return false;

            bool DrawPropField(string propName)
            {
                EditorGUI.BeginChangeCheck();
                var bp = FindPropRel(propName);
                if (bp != null) EditorGUI.PropertyField(position, bp, new GUIContent(""),true);
                else EditorGUI.LabelField(position, propName + " null. " + dataProperty.propertyPath);
                changed = true;
                var c = EditorGUI.EndChangeCheck();
                changed = c;
                return c;
            }

            SerializedProperty FindPropRel(string propName)
            {
                drawn = true;
                var s = StringFormatting.GetBackingFieldName(propName);
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

