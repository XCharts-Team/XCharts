/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Serie), true)]
    public class SerieDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_SerieModuleToggle = new Dictionary<string, bool>();
        private List<bool> m_DataFoldout = new List<bool>();
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
            SerializedProperty m_RadarIndex = prop.FindPropertyRelative("m_RadarIndex");
            SerializedProperty m_MinShow = prop.FindPropertyRelative("m_MinShow");
            SerializedProperty m_MaxShow = prop.FindPropertyRelative("m_MaxShow");
            SerializedProperty m_MaxCache = prop.FindPropertyRelative("m_MaxCache");

            SerializedProperty m_LineStyle = prop.FindPropertyRelative("m_LineStyle");
            SerializedProperty m_ItemStyle = prop.FindPropertyRelative("m_ItemStyle");
            SerializedProperty m_LineArrow = prop.FindPropertyRelative("m_LineArrow");
            SerializedProperty m_LineType = prop.FindPropertyRelative("m_LineType");
            SerializedProperty m_SampleDist = prop.FindPropertyRelative("m_SampleDist");
            SerializedProperty m_SampleType = prop.FindPropertyRelative("m_SampleType");
            SerializedProperty m_SampleAverage = prop.FindPropertyRelative("m_SampleAverage");
            SerializedProperty m_BarType = prop.FindPropertyRelative("m_BarType");
            SerializedProperty m_BarPercentStack = prop.FindPropertyRelative("m_BarPercentStack");
            SerializedProperty m_BarWidth = prop.FindPropertyRelative("m_BarWidth");
            SerializedProperty m_BarGap = prop.FindPropertyRelative("m_BarGap");
            SerializedProperty m_BarZebraWidth = prop.FindPropertyRelative("m_BarZebraWidth");
            SerializedProperty m_BarZebraGap = prop.FindPropertyRelative("m_BarZebraGap");
            SerializedProperty m_AreaStyle = prop.FindPropertyRelative("m_AreaStyle");
            SerializedProperty m_Symbol = prop.FindPropertyRelative("m_Symbol");
            SerializedProperty m_RoseType = prop.FindPropertyRelative("m_RoseType");
            SerializedProperty m_Space = prop.FindPropertyRelative("m_Space");
            SerializedProperty m_Center = prop.FindPropertyRelative("m_Center");
            SerializedProperty m_Radius = prop.FindPropertyRelative("m_Radius");
            SerializedProperty m_Label = prop.FindPropertyRelative("m_Label");
            SerializedProperty m_Emphasis = prop.FindPropertyRelative("m_Emphasis");
            SerializedProperty m_Animation = prop.FindPropertyRelative("m_Animation");
            SerializedProperty m_DataDimension = prop.FindPropertyRelative("m_ShowDataDimension");
            SerializedProperty m_ShowDataName = prop.FindPropertyRelative("m_ShowDataName");
            SerializedProperty m_ShowDataIcon = prop.FindPropertyRelative("m_ShowDataIcon");
            SerializedProperty m_Datas = prop.FindPropertyRelative("m_Data");

            int index = InitToggle(prop);
            string moduleName = "Serie " + index;
            var toggle = ChartEditorHelper.MakeFoldout(ref drawRect, ref m_SerieModuleToggle, prop, moduleName, show);
            if (!toggle)
            {
                drawRect.x = EditorGUIUtility.labelWidth - (EditorGUI.indentLevel - 1) * 15 - 2 + 20;
                drawRect.width = pos.width - drawRect.x + 15;
                EditorGUI.PropertyField(drawRect, type, GUIContent.none);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                var serieType = (SerieType)type.enumValueIndex;

                ++EditorGUI.indentLevel;
                drawRect.x = pos.x;
                drawRect.width = pos.width;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, type);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, name);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, stack);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (serieType == SerieType.Radar)
                {
                    EditorGUI.PropertyField(drawRect, m_RadarIndex);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    EditorGUI.PropertyField(drawRect, m_AxisIndex);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                EditorGUI.PropertyField(drawRect, m_MinShow);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_MaxShow);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_MaxCache);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_MinShow.intValue < 0) m_MinShow.intValue = 0;
                if (m_MinShow.intValue < 0) m_MinShow.intValue = 0;
                if (m_MaxCache.intValue < 0) m_MaxCache.intValue = 0;
                if (serieType == SerieType.Line)
                {
                    EditorGUI.PropertyField(drawRect, m_LineType);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_SampleDist);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_SampleType);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_SampleAverage);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                if (serieType == SerieType.Line
                    || serieType == SerieType.Scatter
                    || serieType == SerieType.EffectScatter
                    || serieType == SerieType.Radar)
                {
                    EditorGUI.PropertyField(drawRect, m_Symbol);
                    drawRect.y += EditorGUI.GetPropertyHeight(m_Symbol);
                }
                if (serieType == SerieType.Bar)
                {
                    EditorGUI.PropertyField(drawRect, m_BarType);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_BarPercentStack);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_BarWidth);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_BarGap);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_BarZebraWidth);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_BarZebraGap);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                if (serieType == SerieType.Pie)
                {
                    EditorGUI.PropertyField(drawRect, m_RoseType);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_Space);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    ChartEditorHelper.MakeTwoField(ref drawRect, pos.width, m_Center, "Center");
                    ChartEditorHelper.MakeTwoField(ref drawRect, pos.width, m_Radius, "Radius");
                }

                EditorGUI.PropertyField(drawRect, m_LineStyle);
                drawRect.y += EditorGUI.GetPropertyHeight(m_LineStyle);
                if (serieType == SerieType.Line)
                {
                    EditorGUI.PropertyField(drawRect, m_LineArrow);
                    drawRect.y += EditorGUI.GetPropertyHeight(m_LineArrow);
                }
                EditorGUI.PropertyField(drawRect, m_ItemStyle);
                drawRect.y += EditorGUI.GetPropertyHeight(m_ItemStyle);
                EditorGUI.PropertyField(drawRect, m_AreaStyle);
                drawRect.y += EditorGUI.GetPropertyHeight(m_AreaStyle);
                EditorGUI.PropertyField(drawRect, m_Label);
                drawRect.y += EditorGUI.GetPropertyHeight(m_Label);
                EditorGUI.PropertyField(drawRect, m_Emphasis);
                drawRect.y += EditorGUI.GetPropertyHeight(m_Emphasis);
                EditorGUI.PropertyField(drawRect, m_Animation);
                drawRect.y += EditorGUI.GetPropertyHeight(m_Animation);
                drawRect.width = EditorGUIUtility.labelWidth + 10;
                m_DataFoldout[index] = EditorGUI.Foldout(drawRect, m_DataFoldout[index], "Data");
                ChartEditorHelper.MakeJsonData(ref drawRect, ref m_ShowJsonDataArea, ref m_JsonDataAreaText, prop, pos.width);
                drawRect.width = pos.width;
                if (m_DataFoldout[index])
                {
                    EditorGUI.indentLevel++;

                    float nameWid = 42;
                    EditorGUI.PropertyField(new Rect(drawRect.x, drawRect.y, pos.width - 2 * nameWid - 2, pos.height), m_DataDimension);
                    var nameRect = new Rect(pos.width - 2 * nameWid + 14, drawRect.y, nameWid, pos.height);
                    if (GUI.Button(nameRect, new GUIContent("Name")))
                    {
                        m_ShowDataName.boolValue = !m_ShowDataName.boolValue;
                    }
                    var iconRect = new Rect(pos.width - nameWid + 14, drawRect.y, nameWid, pos.height);
                    if (GUI.Button(iconRect, new GUIContent("Other")))
                    {
                        m_ShowDataIcon.boolValue = !m_ShowDataIcon.boolValue;
                    }
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    var listSize = m_Datas.arraySize;
                    listSize = EditorGUI.IntField(drawRect, "Size", listSize);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    if (listSize < 0) listSize = 0;
                    if (m_DataDimension.intValue < 1) m_DataDimension.intValue = 1;
                    int dimension = m_DataDimension.intValue;
                    bool showName = m_ShowDataName.boolValue;
                    bool showIcon = m_ShowDataIcon.boolValue;
                    bool showSelected = (serieType == SerieType.Pie);
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
                            DrawDataElement(ref drawRect, dimension, m_Datas, showName, showIcon, showSelected, i, pos.width);
                        }
                        if (num >= 10)
                        {
                            EditorGUI.LabelField(drawRect, "...");
                            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                            DrawDataElement(ref drawRect, dimension, m_Datas, showName, showIcon, showSelected, listSize - 1, pos.width);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < m_Datas.arraySize; i++)
                        {
                            DrawDataElement(ref drawRect, dimension, m_Datas, showName, showIcon, showSelected, i, pos.width);
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                --EditorGUI.indentLevel;
            }
        }

        private void DrawDataElement(ref Rect drawRect, int dimension, SerializedProperty m_Datas, bool showName,
            bool showDetail, bool showSelected, int index, float currentWidth)
        {
            var lastX = drawRect.x;
            var lastWid = drawRect.width;
            var lastFieldWid = EditorGUIUtility.fieldWidth;
            var lastLabelWid = EditorGUIUtility.labelWidth;
            var serieData = m_Datas.GetArrayElementAtIndex(index);
            var sereName = serieData.FindPropertyRelative("m_Name");
            var selected = serieData.FindPropertyRelative("m_Selected");

            var data = serieData.FindPropertyRelative("m_Data");
            var fieldCount = dimension + (showName ? 1 : 0);

            if (fieldCount <= 1)
            {
                while (2 > data.arraySize)
                    data.InsertArrayElementAtIndex(data.arraySize);
                SerializedProperty element = data.GetArrayElementAtIndex(1);
                if (showSelected)
                {
                    drawRect.width = drawRect.width - 18;
                    EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + index));
                    drawRect.x = currentWidth - 45;
                    EditorGUI.PropertyField(drawRect, selected, GUIContent.none);
                    drawRect.x = lastX;
                    drawRect.width = lastWid;
                }
                else
                {
                    EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + index));
                }
                drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                EditorGUI.LabelField(drawRect, "Element " + index);
                Debug.LogError("Element " + index);
                var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15;
                var dataWidTotal = (currentWidth - (startX + 20.5f + 1));
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
                    SerializedProperty element = data.GetArrayElementAtIndex(dimension <= 1 ? 1 : i);
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
            if (showDetail)
            {
                EditorGUI.indentLevel++;
                var m_Icon = serieData.FindPropertyRelative("m_IconStyle");
                EditorGUI.PropertyField(drawRect, m_Icon);
                drawRect.y += EditorGUI.GetPropertyHeight(m_Icon);
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            int index = InitToggle(prop);
            if (!m_SerieModuleToggle[prop.propertyPath])
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                height += 9 * EditorGUIUtility.singleLineHeight + 8 * EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_LineStyle"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_ItemStyle"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_AreaStyle"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Label"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Emphasis"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Animation"));
                SerializedProperty type = prop.FindPropertyRelative("m_Type");
                var serieType = (SerieType)type.enumValueIndex;
                if (serieType == SerieType.Line
                    || serieType == SerieType.Scatter
                    || serieType == SerieType.EffectScatter
                    || serieType == SerieType.Radar)
                {

                    height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Symbol"));
                }
                if (serieType == SerieType.Pie)
                {
                    height += 5 * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.standardVerticalSpacing;
                }
                if (serieType == SerieType.Line)
                {
                    height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_LineArrow"));
                    height += 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
                }
                if (serieType == SerieType.Bar)
                {
                    height += 6 * EditorGUIUtility.singleLineHeight + 5 * EditorGUIUtility.standardVerticalSpacing;
                }
                if (m_DataFoldout[index])
                {
                    SerializedProperty m_Data = prop.FindPropertyRelative("m_Data");
                    height += 2 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
                    int num = m_Data.arraySize;
                    if (num > 30)
                    {
                        num = 11;
                        height += (num + 1) * EditorGUIUtility.singleLineHeight + (num) * EditorGUIUtility.standardVerticalSpacing;
                    }
                    else
                    {
                        height += (num) * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
                    }
                    if (prop.FindPropertyRelative("m_ShowDataIcon").boolValue)
                    {
                        for (int i = 0; i < num; i++)
                        {
                            var item = m_Data.GetArrayElementAtIndex(i);
                            height += EditorGUI.GetPropertyHeight(item.FindPropertyRelative("m_IconStyle"));
                        }
                    }
                }
                if (m_ShowJsonDataArea)
                {
                    height += EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing;
                }
                return height;
            }
        }

        private int InitToggle(SerializedProperty prop)
        {
            int index = 0;
            var sindex = prop.propertyPath.LastIndexOf('[');
            var eindex = prop.propertyPath.LastIndexOf(']');
            if (sindex >= 0 && eindex >= 0)
            {
                var str = prop.propertyPath.Substring(sindex + 1, eindex - sindex - 1);
                int.TryParse(str, out index);
            }
            if (index >= m_DataFoldout.Count)
            {
                m_DataFoldout.Add(false);
            }
            return index;
        }
    }
}