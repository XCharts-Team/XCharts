/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(SerieAnimation), true)]
    public class AnimationDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_AnimationModuleToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_Enable = prop.FindPropertyRelative("m_Enable");
            //SerializedProperty m_Easting = prop.FindPropertyRelative("m_Easting");
            SerializedProperty m_FadeInDuration = prop.FindPropertyRelative("m_FadeInDuration");
            SerializedProperty m_FadeOutDuration = prop.FindPropertyRelative("m_FadeOutDuration");
            SerializedProperty m_DataChangeEnable = prop.FindPropertyRelative("m_DataChangeEnable");
            SerializedProperty m_DataChangeDuration = prop.FindPropertyRelative("m_DataChangeDuration");
            SerializedProperty m_FadeInDelay = prop.FindPropertyRelative("m_FadeInDelay");
            //SerializedProperty m_Threshold = prop.FindPropertyRelative("m_Threshold");
            SerializedProperty m_ActualDuration = prop.FindPropertyRelative("m_ActualDuration");
            SerializedProperty m_FadeOutDelay = prop.FindPropertyRelative("m_FadeOutDelay");
            // SerializedProperty m_CurrDetailProgress = prop.FindPropertyRelative("m_CurrDetailProgress");
            // SerializedProperty m_DestDetailProgress = prop.FindPropertyRelative("m_DestDetailProgress");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_AnimationModuleToggle, prop, null, m_Enable, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_AnimationModuleToggle, prop))
            {
                ++EditorGUI.indentLevel;
                //EditorGUI.PropertyField(drawRect, m_Easting);
                //drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                // EditorGUI.PropertyField(drawRect, m_Threshold);
                // if (m_Threshold.intValue < 0) m_Threshold.intValue = 0;
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FadeInDelay);
                if (m_FadeInDelay.floatValue < 0) m_FadeInDelay.floatValue = 0;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FadeInDuration);
                if (m_FadeInDuration.floatValue < 0) m_FadeInDuration.floatValue = 0;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FadeOutDelay);
                if (m_FadeOutDelay.floatValue < 0) m_FadeOutDelay.floatValue = 0;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FadeOutDuration);
                if (m_FadeOutDuration.floatValue < 0) m_FadeOutDuration.floatValue = 0;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_DataChangeEnable);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_DataChangeDuration);
                if (m_DataChangeDuration.floatValue < 0) m_DataChangeDuration.floatValue = 0;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                // EditorGUI.LabelField(drawRect, "CurrDetailProgress:" + m_CurrDetailProgress.floatValue);
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                // EditorGUI.LabelField(drawRect, "DestDetailProgress:" + m_DestDetailProgress.floatValue);
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.LabelField(drawRect, "Actual duration:" + m_ActualDuration.floatValue + " ms");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (ChartEditorHelper.IsToggle(m_AnimationModuleToggle, prop))
                return 8 * EditorGUIUtility.singleLineHeight + 7 * EditorGUIUtility.standardVerticalSpacing;
            else
                return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}