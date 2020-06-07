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
    [CustomPropertyDrawer(typeof(Location), true)]
    public class LocationDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty align = prop.FindPropertyRelative("m_Align");
            SerializedProperty left = prop.FindPropertyRelative("m_Left");
            SerializedProperty right = prop.FindPropertyRelative("m_Right");
            SerializedProperty top = prop.FindPropertyRelative("m_Top");
            SerializedProperty bottom = prop.FindPropertyRelative("m_Bottom");
            EditorGUI.PropertyField(drawRect, align, new GUIContent("Location"));
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            ++EditorGUI.indentLevel;
            EditorGUI.PropertyField(drawRect, top);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, bottom);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, left);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, right);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            // switch ((Location.Align)align.enumValueIndex)
            // {
            //     case Location.Align.TopCenter:
            //         EditorGUI.PropertyField(drawRect, top);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         break;
            //     case Location.Align.TopLeft:
            //         EditorGUI.PropertyField(drawRect, top);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         EditorGUI.PropertyField(drawRect, left);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         break;
            //     case Location.Align.TopRight:
            //         EditorGUI.PropertyField(drawRect, top);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         EditorGUI.PropertyField(drawRect, right);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         break;
            //     case Location.Align.BottomCenter:
            //         EditorGUI.PropertyField(drawRect, bottom);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         break;
            //     case Location.Align.BottomLeft:
            //         EditorGUI.PropertyField(drawRect, bottom);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         EditorGUI.PropertyField(drawRect, left);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         break;
            //     case Location.Align.BottomRight:
            //         EditorGUI.PropertyField(drawRect, bottom);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         EditorGUI.PropertyField(drawRect, right);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         break;
            //     case Location.Align.Center:
            //         break;
            //     case Location.Align.CenterLeft:
            //         EditorGUI.PropertyField(drawRect, left);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         break;
            //     case Location.Align.CenterRight:
            //         EditorGUI.PropertyField(drawRect, right);
            //         drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //         break;
            // }
            --EditorGUI.indentLevel;
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return 5 * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.standardVerticalSpacing;
            // SerializedProperty align = prop.FindPropertyRelative("m_Align");
            // switch ((Location.Align)align.enumValueIndex)
            // {
            //     case Location.Align.Center:
            //         return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            //     case Location.Align.TopCenter:
            //     case Location.Align.BottomCenter:
            //     case Location.Align.CenterLeft:
            //     case Location.Align.CenterRight:
            //         return 2 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
            //     default:
            //         return 3 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
            // }
        }
    }
}