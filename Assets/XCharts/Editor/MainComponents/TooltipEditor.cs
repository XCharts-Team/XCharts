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
    [ComponentEditor(typeof(Tooltip))]
    public class TooltipEditor : MainComponentEditor<Tooltip>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_Type");
            PropertyField("m_Trigger");
            PropertyField("m_Formatter");
            PropertyField("m_TitleFormatter");
            PropertyField("m_ItemFormatter");
            PropertyField("m_NumericFormatter");
            PropertyField("m_FixedWidth");
            PropertyField("m_FixedHeight");
            PropertyField("m_MinWidth");
            PropertyField("m_MinHeight");
            PropertyField("m_PaddingLeftRight");
            PropertyField("m_PaddingTopBottom");
            PropertyField("m_BackgroundImage");
            PropertyField("m_IgnoreDataDefaultContent");
            PropertyField("m_Offset");
            PropertyField("m_LineStyle");
            PropertyField("m_TextStyle");
            --EditorGUI.indentLevel;
        }
    }
}