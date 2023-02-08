using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(DataZoom))]
    public class DataZoomEditor : MainComponentEditor<DataZoom>
    {
        public override void OnInspectorGUI()
        {
            var m_SupportInside = baseProperty.FindPropertyRelative("m_SupportInside");
            var m_SupportSlider = baseProperty.FindPropertyRelative("m_SupportSlider");
            var m_SupportMarquee = baseProperty.FindPropertyRelative("m_SupportMarquee");
            var m_Start = baseProperty.FindPropertyRelative("m_Start");
            var m_End = baseProperty.FindPropertyRelative("m_End");
            var m_MinShowNum = baseProperty.FindPropertyRelative("m_MinShowNum");
            ++EditorGUI.indentLevel;
            PropertyField("m_Orient");
            PropertyField("m_SupportInside");
            if (m_SupportInside.boolValue)
            {
                PropertyField("m_SupportInsideScroll");
                PropertyField("m_SupportInsideDrag");
            }
            PropertyField(m_SupportSlider);
            PropertyField(m_SupportMarquee);
            PropertyField("m_ZoomLock");
            PropertyField("m_ScrollSensitivity");
            PropertyField("m_RangeMode");
            PropertyField(m_Start);
            PropertyField(m_End);
            PropertyField("m_StartLock");
            PropertyField("m_EndLock");
            PropertyField(m_MinShowNum);
            if (m_Start.floatValue < 0) m_Start.floatValue = 0;
            if (m_End.floatValue > 100) m_End.floatValue = 100;
            if (m_MinShowNum.intValue < 0) m_MinShowNum.intValue = 0;
            if (m_SupportSlider.boolValue)
            {
                PropertyField("m_ShowDataShadow");
                PropertyField("m_ShowDetail");
                PropertyField("m_BackgroundColor");
                PropertyField("m_BorderWidth");
                PropertyField("m_BorderColor");
                PropertyField("m_FillerColor");
                PropertyField("m_Left");
                PropertyField("m_Right");
                PropertyField("m_Top");
                PropertyField("m_Bottom");
                PropertyField("m_LineStyle");
                PropertyField("m_AreaStyle");
                PropertyField("m_LabelStyle");
                PropertyListField("m_XAxisIndexs", true);
                PropertyListField("m_YAxisIndexs", true);
            }
            else
            {
                PropertyListField("m_XAxisIndexs", true);
                PropertyListField("m_YAxisIndexs", true);
            }
            PropertyField("m_MarqueeStyle");
            --EditorGUI.indentLevel;
        }
    }
}