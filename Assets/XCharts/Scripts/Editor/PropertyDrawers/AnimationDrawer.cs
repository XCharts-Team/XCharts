using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Animation), true)]
    public class AnimationDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_AnimationModuleToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_Enable = prop.FindPropertyRelative("m_Enable");
            SerializedProperty m_Easting = prop.FindPropertyRelative("m_Easting");
            SerializedProperty m_Duration = prop.FindPropertyRelative("m_Duration");
            SerializedProperty m_Delay = prop.FindPropertyRelative("m_Delay");
            SerializedProperty m_Threshold = prop.FindPropertyRelative("m_Threshold");
            SerializedProperty m_ActualDuration = prop.FindPropertyRelative("m_ActualDuration");
            SerializedProperty m_CurrDetailProgress = prop.FindPropertyRelative("m_CurrDetailProgress");
            SerializedProperty m_DestDetailProgress = prop.FindPropertyRelative("m_DestDetailProgress");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_AnimationModuleToggle, prop, null, m_Enable, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_AnimationModuleToggle, prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Easting);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Duration);
                if (m_Duration.intValue < 0) m_Duration.intValue = 0;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Delay);
                if (m_Delay.intValue < 0) m_Delay.intValue = 0;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Threshold);
                if (m_Threshold.intValue < 0) m_Threshold.intValue = 0;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.LabelField(drawRect, "CurrDetailProgress:" + m_CurrDetailProgress.floatValue);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.LabelField(drawRect, "DestDetailProgress:" + m_DestDetailProgress.floatValue);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.LabelField(drawRect, "Actual duration:" + m_ActualDuration.intValue + " ms");
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