/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(AngleAxis), true)]
    public class AngleAxisDrawer : AxisDrawer
    {
        protected override void DrawExtended(ref Rect drawRect, SerializedProperty prop)
        {
            SerializedProperty m_StartAngle = prop.FindPropertyRelative("m_StartAngle");
            //SerializedProperty m_Clockwise = prop.FindPropertyRelative("m_Clockwise");
            EditorGUI.PropertyField(drawRect, m_StartAngle);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //EditorGUI.PropertyField(drawRect, m_Clockwise);
            //drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        protected override string GetDisplayName(string displayName)
        {
            if (displayName.StartsWith("Element"))
            {
                displayName = displayName.Replace("Element", "Angle Axis");
            }
            return displayName;
        }

        protected override float GetExtendedHeight()
        {
            return 1 * EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}