using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Axis), true)]
    public class AxisDrawer : PropertyDrawer
    {
        private List<bool> m_AxisModuleToggle = new List<bool>();
        private List<bool> m_DataFoldout = new List<bool>();
        private int m_DataSize = 0;
        private bool m_ShowJsonDataArea = false;
        private string m_JsonDataAreaText;


        protected virtual string GetDisplayName(string displayName)
        {
            return displayName;
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

            int index = InitToggle(prop);
            bool toggle = m_AxisModuleToggle[index];
            m_AxisModuleToggle[index] = ChartEditorHelper.MakeFoldout(ref drawRect, ref toggle,
                GetDisplayName(prop.displayName), m_Show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_AxisModuleToggle[index])
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
                    m_DataFoldout[index] = EditorGUI.Foldout(drawRect, m_DataFoldout[index], "Data");
                    ChartEditorHelper.MakeJsonData(ref drawRect, ref m_ShowJsonDataArea, ref m_JsonDataAreaText, prop, pos.width);
                    drawRect.width = pos.width;
                    if (m_DataFoldout[index])
                    {
                        ChartEditorHelper.MakeList(ref drawRect, ref m_DataSize, m_Data);
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            int index = InitToggle(prop);
            if (!m_AxisModuleToggle[index])
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
                    if (m_DataFoldout[index])
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

        private int InitToggle(SerializedProperty prop)
        {
            int index = 0;
            int.TryParse(prop.displayName.Split(' ')[1],out index);
            if (index >= m_DataFoldout.Count)
            {
                m_DataFoldout.Add(false);
            }
            if (index >= m_AxisModuleToggle.Count)
            {
                m_AxisModuleToggle.Add(false);
            }
            return index;
        }
    }
}