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
    [ComponentEditor(typeof(Title))]
    public class TitleEditor : MainComponentEditor<Title>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_Text");
            PropertyField("m_SubText");
            PropertyField("m_ItemGap");
            PropertyField("m_Location");
            PropertyField("m_TextStyle");
            PropertyField("m_SubTextStyle");
            --EditorGUI.indentLevel;
        }
    }
}