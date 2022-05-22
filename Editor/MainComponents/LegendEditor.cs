using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(Legend))]
    public class LegendEditor : MainComponentEditor<Legend>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_IconType");
            PropertyField("m_ItemWidth");
            PropertyField("m_ItemHeight");
            PropertyField("m_ItemGap");
            PropertyField("m_ItemAutoColor");
            PropertyField("m_ItemOpacity");
            PropertyField("m_SelectedMode");
            PropertyField("m_Orient");
            PropertyField("m_Formatter");
            PropertyField("m_Location");
            PropertyField("m_LabelStyle");
            PropertyListField("m_Icons");
            PropertyListField("m_Colors");
            PropertyListField("m_Data");
            --EditorGUI.indentLevel;
        }
    }
}