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
            PropertyField("m_SelectedMode");
            PropertyField("m_Orient");
            PropertyField("m_Formatter");
            PropertyField("m_Location");
            PropertyField("m_TextStyle");
            PropertyListField("m_Icons");
            PropertyListField("m_Data");
            --EditorGUI.indentLevel;
        }
    }
}