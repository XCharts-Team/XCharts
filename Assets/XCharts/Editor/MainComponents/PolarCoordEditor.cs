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
    [ComponentEditor(typeof(PolarCoord))]
    public class PolarCoordEditor : MainComponentEditor<PolarCoord>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyTwoFiled("m_Center");
            PropertyField("m_Radius");
            PropertyField("m_BackgroundColor");
            --EditorGUI.indentLevel;
        }
    }
}