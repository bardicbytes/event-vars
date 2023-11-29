//alex@bardicbytes.com
//using BardicBytes.BardicFramework.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace BardicBytes.EventVars
{
    [System.Serializable]
    public class EventVarInstanceData
    {
        [HideInInspector]
        [SerializeField]
        private string editorName = "unnamed";

        [SerializeField]
        private EventVar actorInstance;

        [field: SerializeField]
        public EventVar Src { get; protected set; }
        [field: SerializeField] public string StringValue { get; set; }
        [field: SerializeField] public int IntValue { get; set; }
        [field: SerializeField] public float FloatValue { get; set; }
        [field: SerializeField] public bool BoolValue { get; set; }
        [field: SerializeField] public Vector3 Vector3Value { get; set; }
        [field: SerializeField] public Vector2Int Vector2IntValue { get; set; }
        [field: SerializeField] public UnityEngine.Object UnityObjectValue { get; set; }
        [field: SerializeField] public System.Object SystemObjectValue { get; set; }

        private string selector = null;
        [SerializeField]
        private string DEBUG_selstring = "?";

        public EventVar ActorInstance
        {
            get
            {
                Debug.Assert(Application.isPlaying, "runtime only!");
                return actorInstance;
            }
            set => actorInstance = value;
        }

        public EventVarInstanceData()
        {
            editorName = "NEW@" + System.DateTime.Now;
        }

        public EventVarInstanceData(EventVar src)
        {
            ActorInstance = null;
            SetSrc(src);
            editorName = src.name;
        }

        public void RuntimeInitialize(EventVarInstancer owner)
        {
            actorInstance = Src.Clone<EventVar>(owner);
            ActorInstance.SetInitialValue(this);
        }

        private void SetSrc(EventVar evSrc)
        {
            Src = evSrc;
            var selector = evSrc.StoredValueType.FullName;

            if (selector == typeof(string).FullName)
            {
                StringValue = evSrc.UntypedStoredValue as string;
                return;
            }

            if (selector == typeof(int).FullName)
            {
                IntValue = (int)Src.UntypedStoredValue;
                return;

            }
            if (selector == typeof(float).FullName)
            {
                FloatValue = (float)Src.UntypedStoredValue;
                return;
            }
            if (selector == typeof(bool).FullName)
            {
                BoolValue = (bool)Src.UntypedStoredValue;
                return;
            }
            if (selector == typeof(Vector3).FullName)
            {
                Vector3Value = (Vector3)Src.UntypedStoredValue;
                return;
            }
            if (selector == typeof(Vector2Int).FullName)
            {
                Vector2IntValue = (Vector2Int)Src.UntypedStoredValue;
                return;
            }
            if (selector == typeof(UnityEngine.Object).FullName)
            {
                UnityObjectValue = Src.UntypedStoredValue as UnityEngine.Object;
                return;
            }
            if (selector == typeof(System.Object).FullName)
            {
                SystemObjectValue = Src.UntypedStoredValue as System.Object;
                return;
            }
        }

#if UNITY_EDITOR

        private SerializedProperty GetBackingFieldProperty(SerializedProperty root, string name)
        {
            return root.FindPropertyRelative(StringFormatting.GetBackingFieldName(name));
        }

        public void RefreshEditorName() => editorName = string.Format("{0}", Src.name);

        /// <summary>
        /// Draws a property field for just the right serialized property
        /// </summary>
        /// <param name="position">gui rect</param>
        /// <param name="evifProp">event var instance property</param>
        /// <returns>true if changed</returns>
        public virtual bool PropField(Rect position, UnityEditor.SerializedProperty evifProp)
        {
            if (!Src.HasValue)
            {
                return false;
            }
            selector = Src.StoredValueType.FullName;
            DEBUG_selstring = selector + "";
            bool didDraw = false;
            bool changed = false;

            if (selector == typeof(string).FullName && DrawPF("StringValue"))
            {
                var p = FindPropRel("StringValue");
                if (p != null) StringValue = p.stringValue;
            }
            else if (selector == typeof(int).FullName && DrawPF("IntValue"))
            {
                var p = FindPropRel("IntValue");
                if (p != null) IntValue = p.intValue;
            }
            else if (selector == typeof(bool).FullName && DrawPF("BoolValue"))
            {
                var p = FindPropRel("BoolValue");
                if (p != null) BoolValue = p.boolValue;
            }
            else if (selector == typeof(float).FullName && DrawPF("FloatValue"))
            {
                var p = FindPropRel("FloatValue");
                if (p != null) FloatValue = p.floatValue;
            }
            else if (selector == typeof(Vector3).FullName && DrawPF("Vector3Value"))
            {
                var p = FindPropRel("Vector3Value");
                if (p != null) Vector3Value = p.vector3Value;
            }
            else if (selector == typeof(Vector2Int).FullName && DrawPF("Vector2IntValue"))
            {
                var p = FindPropRel("Vector2IntValue");
                if (p != null) Vector2IntValue = p.vector2IntValue;
            }

            if (didDraw) return changed;

            if (selector == typeof(UnityEngine.Object).FullName)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(position, FindPropRel("UnityObjectValue"), true);
                if (EditorGUI.EndChangeCheck())
                {
                    changed = true;
                    var p = FindPropRel("UnityObjectValue");
                    if (p != null) StringValue = p.stringValue;
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
                    if (p != null) StringValue = p.stringValue;
                }
                EditorGUI.EndChangeCheck();
                return changed;
            }

            EditorGUI.LabelField(position, "No Instancing Available for " + Src.StoredValueType.Name);

            return false;

            bool DrawPF(string propname)
            {
                EditorGUI.BeginChangeCheck();
                var bp = FindPropRel(propname);
                if (bp != null) EditorGUI.PropertyField(position, bp);
                else EditorGUI.LabelField(position, propname + " null. " + evifProp.propertyPath);
                didDraw = true;
                var c = EditorGUI.EndChangeCheck();
                changed = c;
                return c;
            }

            SerializedProperty FindPropRel(string propName)
            {
                var s = StringFormatting.GetBackingFieldName(propName);
                SerializedProperty sp = evifProp.Copy();
                if (sp.name == s) return sp;
                while (sp.Next(true))
                {
                    if (sp.name == s) return sp;
                }
                return null;
            }
        }
#endif
    }
}