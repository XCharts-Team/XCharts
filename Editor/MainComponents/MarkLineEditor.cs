using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(MarkLine))]
    public class MarkLineEditor : MainComponentEditor<MarkLine>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_SerieIndex");
            PropertyField("m_OnTop");
            PropertyField("m_Animation");
            PropertyListField("m_Data", true);
            --EditorGUI.indentLevel;
        }
    }

    [CustomPropertyDrawer(typeof(MarkLineData), true)]
    public class MarkLineDataDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "MarkLineData"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "", true))
            {
                ++EditorGUI.indentLevel;
                var type = (MarkLineType) (prop.FindPropertyRelative("m_Type")).enumValueIndex;
                var group = prop.FindPropertyRelative("m_Group").intValue;
                PropertyField(prop, "m_Type");
                PropertyField(prop, "m_Name");
                switch (type)
                {
                    case MarkLineType.None:
                        PropertyField(prop, "m_XPosition");
                        PropertyField(prop, "m_YPosition");
                        PropertyField(prop, "m_XValue");
                        PropertyField(prop, "m_YValue");
                        break;
                    case MarkLineType.Min:
                    case MarkLineType.Max:
                    case MarkLineType.Average:
                    case MarkLineType.Median:
                        PropertyField(prop, "m_Dimension");
                        break;
                }
                PropertyField(prop, "m_Group");
                if (group > 0 && type == MarkLineType.None) PropertyField(prop, "m_ZeroPosition");
                PropertyField(prop, "m_LineStyle");
                PropertyField(prop, "m_StartSymbol");
                PropertyField(prop, "m_EndSymbol");
                PropertyField(prop, "m_Label");
                --EditorGUI.indentLevel;
            }
        }
    }
}