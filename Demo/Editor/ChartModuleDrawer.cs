using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(ChartModule), true)]
    public class ChartModuleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            var lastX = drawRect.x;
            var lastWid = drawRect.width;
            SerializedProperty m_Name = prop.FindPropertyRelative("m_Name");
            SerializedProperty m_Title = prop.FindPropertyRelative("m_Title");
            SerializedProperty m_Selected = prop.FindPropertyRelative("m_Selected");
            SerializedProperty m_Panel = prop.FindPropertyRelative("m_Panel");
            var fieldWid = EditorGUIUtility.currentViewWidth - 30 - 5 - 50 - 90;
            drawRect.width = 15;
            EditorGUI.PropertyField(drawRect,m_Selected,GUIContent.none);
            drawRect.x += 15;
            drawRect.width = 50;
            EditorGUI.PropertyField(drawRect,m_Name,GUIContent.none);
            drawRect.x += 52;
            drawRect.width = fieldWid;
            EditorGUI.PropertyField(drawRect,m_Title,GUIContent.none);
            drawRect.x += fieldWid + 2;
            drawRect.width = 90;
            EditorGUI.PropertyField(drawRect,m_Panel,GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}