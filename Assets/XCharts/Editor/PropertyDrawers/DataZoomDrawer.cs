/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(DataZoom), true)]
    public class DataZoomDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "DataZoom"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Enable"))
            {
                var m_SupportInside = prop.FindPropertyRelative("m_SupportInside");
                var m_SupportSlider = prop.FindPropertyRelative("m_SupportSlider");
                var m_Start = prop.FindPropertyRelative("m_Start");
                var m_End = prop.FindPropertyRelative("m_End");
                var m_MinShowNum = prop.FindPropertyRelative("m_MinShowNum");
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Orient");
                PropertyField(prop, "m_SupportInside");
                if (m_SupportInside.boolValue)
                {
                    PropertyField(prop, "m_SupportInsideScroll");
                    PropertyField(prop, "m_SupportInsideDrag");
                }
                PropertyField(prop, m_SupportSlider);
                PropertyField(prop, "m_ZoomLock");
                PropertyField(prop, "m_ScrollSensitivity");
                PropertyField(prop, "m_RangeMode");
                PropertyField(prop, m_Start);
                PropertyField(prop, m_End);
                PropertyField(prop, m_MinShowNum);
                if (m_Start.floatValue < 0) m_Start.floatValue = 0;
                if (m_End.floatValue > 100) m_End.floatValue = 100;
                if (m_MinShowNum.intValue < 0) m_MinShowNum.intValue = 0;
                if (m_SupportSlider.boolValue)
                {
                    PropertyField(prop, "m_ShowDataShadow");
                    PropertyField(prop, "m_ShowDetail");
                    PropertyField(prop, "m_BackgroundColor");
                    PropertyField(prop, "m_BorderWidth");
                    PropertyField(prop, "m_BorderColor");
                    PropertyField(prop, "m_FillerColor");
                    PropertyField(prop, "m_Left");
                    PropertyField(prop, "m_Right");
                    PropertyField(prop, "m_Top");
                    PropertyField(prop, "m_Bottom");
                    PropertyField(prop, "m_LineStyle");
                    PropertyField(prop, "m_AreaStyle");
                    PropertyListField(prop, "m_XAxisIndexs", true);
                    PropertyListField(prop, "m_YAxisIndexs", true);
                    PropertyField(prop, "m_TextStyle");
                }
                else
                {
                    PropertyListField(prop, "m_XAxisIndexs", true);
                    PropertyListField(prop, "m_YAxisIndexs", true);
                }
                --EditorGUI.indentLevel;
            }
        }
    }
}