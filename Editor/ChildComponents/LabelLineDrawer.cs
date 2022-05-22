using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(LabelLine), true)]
    public class LabelLineDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "LabelLine"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_LineType");
                PropertyField(prop, "m_LineColor");
                PropertyField(prop, "m_LineAngle");
                PropertyField(prop, "m_LineWidth");
                PropertyField(prop, "m_LineGap");
                PropertyField(prop, "m_LineLength1");
                PropertyField(prop, "m_LineLength2");
                PropertyField(prop, "m_StartSymbol");
                PropertyField(prop, "m_EndSymbol");
                --EditorGUI.indentLevel;
            }
        }
    }
}