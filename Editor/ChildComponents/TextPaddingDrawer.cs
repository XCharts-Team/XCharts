using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(Padding), true)]
    public class PaddingDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Padding"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Top");
                PropertyField(prop, "m_Right");
                PropertyField(prop, "m_Bottom");
                PropertyField(prop, "m_Left");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(TextPadding), true)]
    public class TextPaddingDrawer : PaddingDrawer
    {
    }
}