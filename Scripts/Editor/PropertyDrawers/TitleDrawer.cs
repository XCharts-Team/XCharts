using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Title), true)]
    public class TitleDrawer : PropertyDrawer
    {
        private bool m_TitleModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty text = prop.FindPropertyRelative("m_Text");
            SerializedProperty m_TextFontSize = prop.FindPropertyRelative("m_TextFontSize");
            SerializedProperty subText = prop.FindPropertyRelative("m_SubText");
            SerializedProperty m_SubTextFontSize = prop.FindPropertyRelative("m_SubTextFontSize");
            SerializedProperty m_ItemGap = prop.FindPropertyRelative("m_ItemGap");
            SerializedProperty location = prop.FindPropertyRelative("m_Location");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_TitleModuleToggle, "Title", show);
            ++EditorGUI.indentLevel;
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_TitleModuleToggle)
            {
                EditorGUI.PropertyField(drawRect, text);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_TextFontSize, new GUIContent("Font Size"));
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, subText);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_SubTextFontSize, new GUIContent("Font Size"));
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ItemGap, new GUIContent("Item Gap"));
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, location);
            }
            --EditorGUI.indentLevel;
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (m_TitleModuleToggle)
            {
                height += 5 * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.standardVerticalSpacing;
                SerializedProperty location = prop.FindPropertyRelative("m_Location");
                height += EditorGUI.GetPropertyHeight(location);
            }
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}