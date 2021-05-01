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
    [CustomPropertyDrawer(typeof(Settings), true)]
    public class SettingsDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Settings"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, ""))
            {
                var btnWidth = 50;
                var btnRect = new Rect(pos.x + pos.width - btnWidth, pos.y, btnWidth, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(btnRect, new GUIContent("Reset", "Reset to default settings")))
                {
                    var chart = prop.serializedObject.targetObject as BaseChart;
                    chart.settings.Reset();
                }
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_ReversePainter");
                PropertyField(prop, "m_MaxPainter");
                PropertyField(prop, "m_BasePainterMaterial");
                PropertyField(prop, "m_SeriePainterMaterial");
                PropertyField(prop, "m_TopPainterMaterial");
                PropertyField(prop, "m_LineSmoothStyle");
                PropertyField(prop, "m_LineSmoothness");
                PropertyField(prop, "m_LineSegmentDistance");
                PropertyField(prop, "m_CicleSmoothness");
                PropertyField(prop, "m_LegendIconLineWidth");
                PropertyListField(prop, "m_LegendIconCornerRadius", true);
                --EditorGUI.indentLevel;
            }
        }
    }
}