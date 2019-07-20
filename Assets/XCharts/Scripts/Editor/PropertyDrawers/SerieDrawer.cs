using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Serie), true)]
    public class SerieDrawer : PropertyDrawer
    {
        private List<bool> m_SerieModuleToggle = new List<bool>();
        private List<bool> m_DataFoldout = new List<bool>();
        private int m_DataSize = 0;
        private bool m_ShowJsonDataArea = false;
        private string m_JsonDataAreaText;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty type = prop.FindPropertyRelative("m_Type");
            SerializedProperty name = prop.FindPropertyRelative("m_Name");
            SerializedProperty stack = prop.FindPropertyRelative("m_Stack");
            SerializedProperty m_AxisIndex = prop.FindPropertyRelative("m_AxisIndex");
            SerializedProperty m_Symbol = prop.FindPropertyRelative("m_Symbol");
            SerializedProperty m_SymbolSize = prop.FindPropertyRelative("m_SymbolSize");
            SerializedProperty m_SymbolSelectedSize = prop.FindPropertyRelative("m_SymbolSelectedSize");
            SerializedProperty m_TwoDimensionData = prop.FindPropertyRelative("m_TwoDimensionData");
            SerializedProperty m_XData = prop.FindPropertyRelative("m_XData");
            SerializedProperty m_YData = prop.FindPropertyRelative("m_YData");

            int index = InitToggle(prop);
            string moduleName = "Serie " + index;
            bool toggle = m_SerieModuleToggle[index];
            m_SerieModuleToggle[index] = ChartEditorHelper.MakeFoldout(ref drawRect, ref toggle, moduleName, show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_SerieModuleToggle[index])
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, type);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, name);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, stack);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_AxisIndex);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (type.enumValueIndex == (int)SerieType.Line || type.enumValueIndex == (int)SerieType.Scatter)
                {
                    EditorGUI.PropertyField(drawRect, m_Symbol);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_SymbolSize);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_SymbolSelectedSize);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                EditorGUI.PropertyField(drawRect, m_TwoDimensionData);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.width = EditorGUIUtility.labelWidth + 10;
                m_DataFoldout[index] = EditorGUI.Foldout(drawRect, m_DataFoldout[index], "Data");
                ChartEditorHelper.MakeJsonData(ref drawRect, ref m_ShowJsonDataArea, ref m_JsonDataAreaText, prop);
                drawRect.width = pos.width;
                if (m_DataFoldout[index])
                {
                    if (m_TwoDimensionData.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        var listSize = m_YData.arraySize;
                        listSize = EditorGUI.IntField(drawRect, "Size", listSize);
                        if (listSize < 0) listSize = 0;
                        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        if (listSize != m_XData.arraySize)
                        {
                            while (listSize > m_XData.arraySize)
                                m_XData.InsertArrayElementAtIndex(m_XData.arraySize);
                            while (listSize < m_XData.arraySize)
                                m_XData.DeleteArrayElementAtIndex(m_XData.arraySize - 1);
                        }
                        if (listSize != m_YData.arraySize)
                        {
                            while (listSize > m_YData.arraySize)
                                m_YData.InsertArrayElementAtIndex(m_YData.arraySize);
                            while (listSize < m_YData.arraySize)
                                m_YData.DeleteArrayElementAtIndex(m_YData.arraySize - 1);
                        }
                        if (listSize > 30)
                        {
                            int num = listSize > 10 ? 10 : listSize;
                            for (int i = 0; i < num; i++)
                            {
                                DrawTwoDimensionDataElement(ref drawRect, m_XData, m_YData, i);
                            }
                            if (num >= 10)
                            {
                                EditorGUI.LabelField(drawRect, "...");
                                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                                DrawTwoDimensionDataElement(ref drawRect, m_XData, m_YData, listSize - 1);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < m_YData.arraySize; i++)
                            {
                                DrawTwoDimensionDataElement(ref drawRect, m_XData, m_YData, i);
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                    else
                    {
                        ChartEditorHelper.MakeList(ref drawRect, ref m_DataSize, m_YData);
                    }
                }
                --EditorGUI.indentLevel;
            }
        }

        private void DrawTwoDimensionDataElement(ref Rect drawRect, SerializedProperty m_Data1,
            SerializedProperty m_Data2, int i)
        {
            var lastX = drawRect.x;
            SerializedProperty element1 = m_Data1.GetArrayElementAtIndex(i);
            SerializedProperty element2 = m_Data2.GetArrayElementAtIndex(i);
            EditorGUI.LabelField(drawRect, "Element " + i);
            var startX = EditorGUIUtility.labelWidth - (EditorGUI.indentLevel - 1) * 15 - 1;
            var dataWid = (EditorGUIUtility.currentViewWidth - startX) / 2 + 15;
            drawRect.x = startX;
            drawRect.width = dataWid;
            element1.floatValue = EditorGUI.FloatField(drawRect, element1.floatValue);
            drawRect.x += dataWid - 35;
            element2.floatValue = EditorGUI.FloatField(drawRect, element2.floatValue);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            drawRect.x = lastX;
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            int index = InitToggle(prop);
            if (!m_SerieModuleToggle[index])
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                height += 7 * EditorGUIUtility.singleLineHeight + 6 * EditorGUIUtility.standardVerticalSpacing;
                SerializedProperty type = prop.FindPropertyRelative("m_Type");
                if (type.enumValueIndex == (int)SerieType.Line || type.enumValueIndex == (int)SerieType.Scatter)
                {
                    height += 3 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
                }
                if (m_DataFoldout[index])
                {
                    SerializedProperty m_Data = prop.FindPropertyRelative("m_YData");
                    int num = m_Data.arraySize + 1;
                    if (num > 30) num = 13;
                    height += num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
                }
                if (m_ShowJsonDataArea)
                {
                    height += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
                }
                return height;
            }
        }

        private int InitToggle(SerializedProperty prop)
        {
            int index = 0;
            int.TryParse(prop.displayName.Split(' ')[1], out index);
            if (index >= m_DataFoldout.Count)
            {
                m_DataFoldout.Add(false);
            }
            if (index >= m_SerieModuleToggle.Count)
            {
                m_SerieModuleToggle.Add(false);
            }
            return index;
        }
    }
}