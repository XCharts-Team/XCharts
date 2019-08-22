using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(AxisName), true)]
    public class AxisNameDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_AxisNameToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Name = prop.FindPropertyRelative("m_Name");
            SerializedProperty m_Location = prop.FindPropertyRelative("m_Location");
            SerializedProperty m_Offset = prop.FindPropertyRelative("m_Offset");
            SerializedProperty m_Rotate = prop.FindPropertyRelative("m_Rotate");
            SerializedProperty m_Color = prop.FindPropertyRelative("m_Color");
            SerializedProperty m_FontSize = prop.FindPropertyRelative("m_FontSize");
            SerializedProperty m_FontStyle = prop.FindPropertyRelative("m_FontStyle");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_AxisNameToggle, prop, "Axis Name", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_AxisNameToggle, prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Name);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Location);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Offset);
                drawRect.y += EditorGUI.GetPropertyHeight(m_Offset);
                // EditorGUI.LabelField(drawRect, "Offset");
                // var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15;
                // var tempWidth = (pos.width - startX + 35) / 2;
                // var xRect = new Rect(startX, drawRect.y, tempWidth, drawRect.height);
                // var yRect = new Rect(xRect.x + tempWidth - 20, drawRect.y, tempWidth, drawRect.height);
                // var x = EditorGUI.FloatField(xRect, m_Offset.vector2Value.x);
                // var y = EditorGUI.FloatField(yRect, m_Offset.vector2Value.y);
                // m_Offset.vector2Value = new Vector2(x,y);
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Rotate);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Color);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FontSize);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FontStyle);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (ChartEditorHelper.IsToggle(m_AxisNameToggle, prop))
            {
                height += 7 * EditorGUIUtility.singleLineHeight + 6 * EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }
    }
}