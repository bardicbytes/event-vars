//alex@bardicbytes.com
using BardicBytes.EventVars;
using UnityEditor;
using UnityEngine;

namespace BardicBytes.BardicFrameworkEditor
{
    [CustomPropertyDrawer(typeof(EventVarInstanceData), true)]
    public class EVInstConfigDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty topProp, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, topProp);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var srcProp = topProp.FindPropertyRelative("src");

            //var r = new Rect(position.x, position.y, 75, position.height);
            //var r2 = new Rect(position.x + r.width+5, position.y, position.width - r.width - 5, position.height);

            EventVarInstanceData bc = (EventVarInstanceData)topProp.managedReferenceValue;
            
            try
            {
                bc.PropField(position, topProp);
            }
            catch (System.NullReferenceException e)
            {
                EditorGUILayout.LabelField(label.text + " NullRef Caught \n" + e.Message);
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}