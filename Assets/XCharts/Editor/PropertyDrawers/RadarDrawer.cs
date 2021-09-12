/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Radar), true)]
    public class RadarDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Radar"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Shape");
                PropertyField(prop, "m_PositionType");
                PropertyTwoFiled(prop, "m_Center");
                PropertyField(prop, "m_Radius");
                PropertyField(prop, "m_SplitNumber");
                PropertyField(prop, "m_CeilRate");
                PropertyField(prop, "m_IsAxisTooltip");
                PropertyField(prop, "m_OutRangeColor");
                PropertyField(prop, "m_ConnectCenter");
                PropertyField(prop, "m_LineGradient");
                PropertyField(prop, "m_AxisLine");
                PropertyField(prop, "m_SplitLine");
                PropertyField(prop, "m_SplitArea");
                PropertyField(prop, "m_IndicatorList");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(Radar.Indicator), true)]
    public class RadarIndicatorDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Indicator"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, ""))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Name");
                PropertyField(prop, "m_Min");
                PropertyField(prop, "m_Max");
                PropertyTwoFiled(prop, "m_Range");
                PropertyField(prop, "m_TextStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}