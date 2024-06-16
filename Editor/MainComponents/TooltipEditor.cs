using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(Tooltip))]
    public class TooltipEditor : MainComponentEditor<Tooltip>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_Type");
            PropertyField("m_Trigger");
            PropertyField("m_TriggerOn");
            PropertyField("m_Position");
            PropertyField("m_FixedX");
            PropertyField("m_FixedY");
            PropertyField("m_Offset");
            PropertyField("m_ShowContent");
            PropertyField("m_AlwayShowContent");
            PropertyField("m_TitleFormatter");
            PropertyField("m_ItemFormatter");
            PropertyField("m_NumericFormatter");
            PropertyFiledMore(() =>
            {
                PropertyField("m_TitleHeight");
                PropertyField("m_ItemHeight");
                PropertyField("m_Marker");
                PropertyField("m_BorderWidth");
                PropertyField("m_BorderColor");
                PropertyField("m_PaddingLeftRight");
                PropertyField("m_PaddingTopBottom");
                PropertyField("m_BackgroundImage");
                PropertyField("m_BackgroundType");
                PropertyField("m_BackgroundColor");
                PropertyField("m_FixedWidth");
                PropertyField("m_FixedHeight");
                PropertyField("m_MinWidth");
                PropertyField("m_MinHeight");
                PropertyField("m_IgnoreDataDefaultContent");
            });
            PropertyField("m_LineStyle");
            PropertyField("m_TitleLabelStyle");
            PropertyListField("m_ContentLabelStyles");
            --EditorGUI.indentLevel;
        }
    }
}