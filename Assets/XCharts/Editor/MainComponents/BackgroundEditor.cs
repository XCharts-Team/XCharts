
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEditor;

namespace XCharts
{
    [ComponentEditor(typeof(Background))]
    internal sealed class BackgroundEditor : MainComponentEditor<Background>
    {
        public override void OnInspectorGUI()
        {

            ++EditorGUI.indentLevel;
            PropertyField("m_Image");
            PropertyField("m_ImageType");
            PropertyField("m_ImageColor");
            PropertyField("m_HideThemeBackgroundColor");
            --EditorGUI.indentLevel;
        }
    }
}