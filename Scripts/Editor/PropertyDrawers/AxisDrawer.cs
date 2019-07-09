using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

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
            SerializedProperty m_AxisLabel = prop.FindPropertyRelative("m_AxisLabel");
            SerializedProperty m_ShowSplitLine = prop.FindPropertyRelative("m_ShowSplitLine");
            SerializedProperty m_SplitLineType = prop.FindPropertyRelative("m_SplitLineType");
            SerializedProperty m_BoundaryGap = prop.FindPropertyRelative("m_BoundaryGap");
            SerializedProperty m_Data = prop.FindPropertyRelative("m_Data");
            SerializedProperty m_AxisLine = prop.FindPropertyRelative("m_AxisLine");
            SerializedProperty m_AxisName = prop.FindPropertyRelative("m_AxisName");
            SerializedProperty m_AxisTick = prop.FindPropertyRelative("m_AxisTick");
            SerializedProperty m_SplitArea = prop.FindPropertyRelative("m_SplitArea");
            SerializedProperty m_MinMaxType = prop.FindPropertyRelative("m_MinMaxType");
            SerializedProperty m_Min = prop.FindPropertyRelative("m_Min");
            SerializedProperty m_Max = prop.FindPropertyRelative("m_Max");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_AxisModuleToggle, prop.displayName, m_Show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_AxisModuleToggle)
            {
                Axis.AxisType type = (Axis.AxisType)m_Type.enumValueIndex;
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(drawRect, m_Type);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (type == Axis.AxisType.Value)
                {
                    EditorGUI.PropertyField(drawRect, m_MinMaxType);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    Axis.AxisMinMaxType minMaxType = (Axis.AxisMinMaxType)m_MinMaxType.enumValueIndex;
                    switch (minMaxType)
                    {
                        case Axis.AxisMinMaxType.Default:
                            break;
                        case Axis.AxisMinMaxType.MinMax:
                            break;
                        case Axis.AxisMinMaxType.Custom:
                            EditorGUI.indentLevel++;
                            EditorGUI.PropertyField(drawRect, m_Min);
                            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                            EditorGUI.PropertyField(drawRect, m_Max);
                            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                            EditorGUI.indentLevel--;
                            break;
                    }
                }
                EditorGUI.PropertyField(drawRect, m_SplitNumber);
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
                EditorGUI.PropertyField(drawRect, m_AxisLine);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.y += EditorGUI.GetPropertyHeight(m_AxisLine);
                EditorGUI.PropertyField(drawRect, m_AxisName);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.y += EditorGUI.GetPropertyHeight(m_AxisName);
                EditorGUI.PropertyField(drawRect, m_AxisTick);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.y += EditorGUI.GetPropertyHeight(m_AxisTick);
                EditorGUI.PropertyField(drawRect, m_AxisLabel);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.y += EditorGUI.GetPropertyHeight(m_AxisLabel);
                EditorGUI.PropertyField(drawRect, m_SplitArea);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.y += EditorGUI.GetPropertyHeight(m_SplitArea);

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
                SerializedProperty m_AxisLine = prop.FindPropertyRelative("m_AxisLine");
                SerializedProperty m_AxisName = prop.FindPropertyRelative("m_AxisName");
                SerializedProperty m_AxisLabel = prop.FindPropertyRelative("m_AxisLabel");
                SerializedProperty m_SplitArea = prop.FindPropertyRelative("m_SplitArea");
                float height = 0;
                height += 10 * EditorGUIUtility.singleLineHeight + 9 * EditorGUIUtility.standardVerticalSpacing;
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
                else if (type == Axis.AxisType.Value)
                {
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    SerializedProperty m_MinMaxType = prop.FindPropertyRelative("m_MinMaxType");
                    if (m_MinMaxType.enumValueIndex == (int)Axis.AxisMinMaxType.Custom)
                    {
                        height += EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                height += EditorGUI.GetPropertyHeight(m_AxisName);
                height += EditorGUI.GetPropertyHeight(m_AxisLine);
                height += EditorGUI.GetPropertyHeight(m_AxisTick);
                height += EditorGUI.GetPropertyHeight(m_AxisLabel);
                height += EditorGUI.GetPropertyHeight(m_SplitArea);
                return height;
            }

        }
    }
}