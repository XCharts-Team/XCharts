using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Axis), true)]
    public class AxisDrawer : PropertyDrawer
    {
        private ReorderableList m_DataList;
        private bool m_DataFoldout = false;
        private int m_DataSize = 0;
        private bool m_ShowJsonDataArea = false;
        private string m_JsonDataAreaText;
        private bool m_AxisModuleToggle = false;

        private void InitReorderableList(SerializedProperty prop)
        {
            if (m_DataList == null)
            {
                SerializedProperty data = prop.FindPropertyRelative("m_Data");
                m_DataList = new ReorderableList(data.serializedObject, data, false, false, true, true);
                m_DataList.elementHeight = EditorGUIUtility.singleLineHeight;
                m_DataList.drawHeaderCallback += delegate (Rect rect)
                {
                    EditorGUI.LabelField(rect, data.displayName);
                };

                m_DataList.drawElementCallback = delegate (Rect rect, int index, bool isActive, bool isFocused)
                {
                    EditorGUI.PropertyField(rect, data.GetArrayElementAtIndex(index), true);
                };
            }
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_Show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Type = prop.FindPropertyRelative("m_Type");
            SerializedProperty m_SplitNumber = prop.FindPropertyRelative("m_SplitNumber");
            SerializedProperty m_TextRotation = prop.FindPropertyRelative("m_TextRotation");
            SerializedProperty m_ShowSplitLine = prop.FindPropertyRelative("m_ShowSplitLine");
            SerializedProperty m_SplitLineType = prop.FindPropertyRelative("m_SplitLineType");
            SerializedProperty m_BoundaryGap = prop.FindPropertyRelative("m_BoundaryGap");
            SerializedProperty m_Data = prop.FindPropertyRelative("m_Data");
            SerializedProperty m_AxisTick = prop.FindPropertyRelative("m_AxisTick");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_AxisModuleToggle, prop.displayName, m_Show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_AxisModuleToggle)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(drawRect, m_Type);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SplitNumber);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_TextRotation);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_ShowSplitLine.boolValue)
                {
                    drawRect.width = EditorGUIUtility.labelWidth + 10;
                    EditorGUI.PropertyField(drawRect, m_ShowSplitLine);
                    //drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    drawRect.x = EditorGUIUtility.labelWidth + 35;
                    drawRect.width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 55;
                    EditorGUI.PropertyField(drawRect, m_SplitLineType, GUIContent.none);
                    drawRect.x = pos.x;
                    drawRect.width = pos.width;
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    EditorGUI.PropertyField(drawRect, m_ShowSplitLine);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                EditorGUI.PropertyField(drawRect, m_BoundaryGap);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_AxisTick);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.y += EditorGUI.GetPropertyHeight(m_AxisTick);
                Axis.AxisType type = (Axis.AxisType)m_Type.enumValueIndex;
                if (type == Axis.AxisType.Category)
                {
                    drawRect.width = EditorGUIUtility.labelWidth + 10;
                    m_DataFoldout = EditorGUI.Foldout(drawRect, m_DataFoldout, "Data");
                    ChartEditorHelper.MakeJsonData(ref drawRect, ref m_ShowJsonDataArea, ref m_JsonDataAreaText, prop);
                    drawRect.width = pos.width;
                    if (m_DataFoldout)
                    {
                        ChartEditorHelper.MakeList(ref drawRect, ref m_DataSize, m_Data);
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (!m_AxisModuleToggle)
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                SerializedProperty m_Type = prop.FindPropertyRelative("m_Type");
                SerializedProperty m_AxisTick = prop.FindPropertyRelative("m_AxisTick");
                float height = 0;
                height += 7 * EditorGUIUtility.singleLineHeight + 6 * EditorGUIUtility.standardVerticalSpacing;
                Axis.AxisType type = (Axis.AxisType)m_Type.enumValueIndex;
                if (type == Axis.AxisType.Category)
                {
                    if (m_DataFoldout)
                    {
                        SerializedProperty m_Data = prop.FindPropertyRelative("m_Data");
                        int num = m_Data.arraySize + 2;
                        if (num > 50) num = 13;
                        height += num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
                        height += EditorGUIUtility.standardVerticalSpacing;
                    }
                    else
                    {
                        height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
                    }
                    if (m_ShowJsonDataArea)
                    {
                        height += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                height += EditorGUI.GetPropertyHeight(m_AxisTick);
                return height;
            }
                
        }
    }
}