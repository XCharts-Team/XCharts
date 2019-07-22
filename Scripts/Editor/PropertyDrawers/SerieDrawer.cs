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
            SerializedProperty m_DataDimension = prop.FindPropertyRelative("m_ShowDataDimension");
            SerializedProperty m_ShowDataName = prop.FindPropertyRelative("m_ShowDataName");
            SerializedProperty m_Datas = prop.FindPropertyRelative("m_Data");

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
                if (type.enumValueIndex == (int)SerieType.Line
                    || type.enumValueIndex == (int)SerieType.Scatter
                    || type.enumValueIndex == (int)SerieType.EffectScatter)
                {
                    EditorGUI.PropertyField(drawRect, m_Symbol);
                    drawRect.y += EditorGUI.GetPropertyHeight(m_Symbol);
                }

                drawRect.width = EditorGUIUtility.labelWidth + 10;
                m_DataFoldout[index] = EditorGUI.Foldout(drawRect, m_DataFoldout[index], "Data");
                ChartEditorHelper.MakeJsonData(ref drawRect, ref m_ShowJsonDataArea, ref m_JsonDataAreaText, prop);
                drawRect.width = pos.width;
                if (m_DataFoldout[index])
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.PropertyField(drawRect, m_ShowDataName);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_DataDimension);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    var listSize = m_Datas.arraySize;
                    listSize = EditorGUI.IntField(drawRect, "Size", listSize);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    if (listSize < 0) listSize = 0;
                    if (m_DataDimension.intValue < 1) m_DataDimension.intValue = 1;
                    int dimension = m_DataDimension.intValue;
                    bool showName = m_ShowDataName.boolValue;
                    if (listSize != m_Datas.arraySize)
                    {
                        while (listSize > m_Datas.arraySize)
                            m_Datas.InsertArrayElementAtIndex(m_Datas.arraySize);
                        while (listSize < m_Datas.arraySize)
                            m_Datas.DeleteArrayElementAtIndex(m_Datas.arraySize - 1);
                    }
                    if (listSize > 30)
                    {
                        int num = listSize > 10 ? 10 : listSize;
                        for (int i = 0; i < num; i++)
                        {
                            DrawDataElement(ref drawRect, dimension, m_Datas, showName, i);
                        }
                        if (num >= 10)
                        {
                            EditorGUI.LabelField(drawRect, "...");
                            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                            DrawDataElement(ref drawRect, dimension, m_Datas, showName, listSize - 1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < m_Datas.arraySize; i++)
                        {
                            DrawDataElement(ref drawRect, dimension, m_Datas, showName, i);
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                --EditorGUI.indentLevel;
            }
        }

        private void DrawDataElement(ref Rect drawRect, int dimension, SerializedProperty m_Datas, bool showName, int index)
        {
            var lastX = drawRect.x;
            var lastWid = drawRect.width;
            var lastFieldWid = EditorGUIUtility.fieldWidth;
            var lastLabelWid = EditorGUIUtility.labelWidth;
            var serieData = m_Datas.GetArrayElementAtIndex(index);
            var sereName = serieData.FindPropertyRelative("m_Name");
            var data = serieData.FindPropertyRelative("m_Data");
            var fieldCount = dimension + (showName ? 1 : 0);

            if (fieldCount <= 1)
            {
                while (2 > data.arraySize)
                    data.InsertArrayElementAtIndex(data.arraySize);
                SerializedProperty element = data.GetArrayElementAtIndex(1);
                EditorGUI.PropertyField(drawRect, element);
                drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                EditorGUI.LabelField(drawRect, "Element " + index);
                var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15 - 1;
                var dataWidTotal = (EditorGUIUtility.currentViewWidth - (startX + EditorGUI.indentLevel * 15 + 1) - 5);
                var dataWid = dataWidTotal / fieldCount;
                var xWid = dataWid - 4;
                for (int i = 0; i < dimension; i++)
                {
                    if (i >= data.arraySize - 1)
                    {
                        data.InsertArrayElementAtIndex(data.arraySize);
                    }
                    drawRect.x = startX + i * xWid;
                    drawRect.width = dataWid + 40;
                    SerializedProperty element = data.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(drawRect, element, GUIContent.none);
                }
                if (showName)
                {
                    drawRect.x = startX + (fieldCount - 1) * xWid;
                    drawRect.width = dataWid + 40;
                    EditorGUI.PropertyField(drawRect, sereName, GUIContent.none);
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.x = lastX;
                drawRect.width = lastWid;
                EditorGUIUtility.fieldWidth = lastFieldWid;
                EditorGUIUtility.labelWidth = lastLabelWid;
            }
            SerializedProperty element1 = data.GetArrayElementAtIndex(0);
            SerializedProperty element2 = data.GetArrayElementAtIndex(1);
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
                height += 6 * EditorGUIUtility.singleLineHeight + 5 * EditorGUIUtility.standardVerticalSpacing;
                SerializedProperty type = prop.FindPropertyRelative("m_Type");
                if (type.enumValueIndex == (int)SerieType.Line 
                    || type.enumValueIndex == (int)SerieType.Scatter
                    || type.enumValueIndex == (int)SerieType.EffectScatter)
                {

                    height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Symbol"));
                }
                if (m_DataFoldout[index])
                {
                    SerializedProperty m_Data = prop.FindPropertyRelative("m_Data");
                    int num = m_Data.arraySize + 3;
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
            var temp = prop.displayName.Split(' ');
            if (temp == null || temp.Length < 2)
            {
                //Debug.LogError("SERIE:"+prop.name+","+prop.displayName+","+prop.FindPropertyRelative("m_Name").stringValue);
                index = 0;
            }
            else
            {
                int.TryParse(temp[1], out index);
            }
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