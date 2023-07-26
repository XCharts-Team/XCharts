using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{

    [CustomPropertyDrawer(typeof(MLValue), true)]
    public class MLValueDrawer : BasePropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Percent = prop.FindPropertyRelative("m_Type");
            SerializedProperty m_Color = prop.FindPropertyRelative("m_Value");

            ChartEditorHelper.MakeTwoField(ref drawRect, drawRect.width, m_Percent, m_Color, prop.displayName);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}