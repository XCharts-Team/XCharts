/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Serie), true)]
    public class SerieDrawer : BasePropertyDrawer
    {
        private bool m_IsPolar = false;
        private List<bool> m_DataFoldout = new List<bool>();
        public override string ClassName { get { return "Serie"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            pos.width -= 9;
            base.OnGUI(pos, prop, label);
            var chart = prop.serializedObject.targetObject as BaseChart;
            var type = prop.FindPropertyRelative("m_Type");
            var serieType = (SerieType)type.enumValueIndex;
            if (!MakeFoldout(prop, "m_Show"))
            {
                var orderButton = 48;
                var gap = 4;
                var drawRect = pos;
                drawRect.x += EditorGUIUtility.labelWidth + gap;
                drawRect.width = pos.width - drawRect.x + ChartEditorHelper.BOOL_WIDTH - orderButton;
                type.enumValueIndex = EditorGUI.Popup(drawRect, (int)serieType, GetChartSerieTypeNames(chart));
            }
            else
            {
                m_IsPolar = chart is PolarChart;
                ++EditorGUI.indentLevel;

                type.enumValueIndex = EditorGUI.Popup(m_DrawRect, "Type", (int)serieType, GetChartSerieTypeNames(chart));
                var hig = EditorGUI.GetPropertyHeight(prop);
                m_DrawRect.y += hig;
                m_Heights[m_KeyName] += hig;

                PropertyField(prop, "m_InsertDataToHead");
                PropertyField(prop, "m_Name");
                switch (serieType)
                {
                    case SerieType.Line:
                        PropertyField(prop, "m_Stack");
                        if (m_IsPolar)
                        {
                            PropertyField(prop, "m_PolarIndex");
                        }
                        else
                        {
                            PropertyField(prop, "m_XAxisIndex");
                            PropertyField(prop, "m_YAxisIndex");
                        }
                        PropertyFieldLimitMin(prop, "m_MinShow", 0);
                        PropertyFieldLimitMin(prop, "m_MaxShow", 0);
                        PropertyFieldLimitMin(prop, "m_MaxCache", 0);
                        PropertyField(prop, "m_LineType");
                        PropertyField(prop, "m_SampleDist");
                        PropertyField(prop, "m_SampleType");
                        PropertyField(prop, "m_SampleAverage");
                        PropertyField(prop, "m_Clip");
                        PropertyField(prop, "m_Ignore");
                        PropertyField(prop, "m_IgnoreValue");
                        PropertyField(prop, "m_IgnoreLineBreak");
                        PropertyField(prop, "m_ShowAsPositiveNumber");
                        PropertyField(prop, "m_Large");
                        PropertyField(prop, "m_LargeThreshold");
                        PropertyField(prop, "m_Symbol");
                        PropertyField(prop, "m_LineStyle");
                        PropertyField(prop, "m_LineArrow");
                        PropertyField(prop, "m_AreaStyle");
                        PropertyField(prop, "m_MarkLine");
                        break;
                    case SerieType.Bar:
                        PropertyField(prop, "m_Stack");
                        if (m_IsPolar)
                        {
                            PropertyField(prop, "m_PolarIndex");
                        }
                        else
                        {
                            PropertyField(prop, "m_XAxisIndex");
                            PropertyField(prop, "m_YAxisIndex");
                        }
                        PropertyFieldLimitMin(prop, "m_MinShow", 0);
                        PropertyFieldLimitMin(prop, "m_MaxShow", 0);
                        PropertyFieldLimitMin(prop, "m_MaxCache", 0);
                        PropertyField(prop, "m_BarType");
                        PropertyField(prop, "m_BarPercentStack");
                        PropertyField(prop, "m_BarWidth");
                        PropertyField(prop, "m_BarGap");
                        PropertyField(prop, "m_BarZebraWidth");
                        PropertyField(prop, "m_BarZebraGap");
                        PropertyField(prop, "m_Clip");
                        PropertyField(prop, "m_Ignore");
                        PropertyField(prop, "m_IgnoreValue");
                        PropertyField(prop, "m_ShowAsPositiveNumber");
                        PropertyField(prop, "m_Large");
                        PropertyField(prop, "m_LargeThreshold");
                        PropertyField(prop, "m_MarkLine");
                        break;
                    case SerieType.Pie:
                        PropertyField(prop, "m_RoseType");
                        PropertyField(prop, "m_Space");
                        PropertyTwoFiled(prop, "m_Center");
                        PropertyTwoFiled(prop, "m_Radius");
                        PropertyField(prop, "m_MinAngle");
                        PropertyField(prop, "m_RoundCap");
                        PropertyField(prop, "m_Ignore");
                        PropertyField(prop, "m_IgnoreValue");
                        PropertyField(prop, "m_AvoidLabelOverlap");
                        break;
                    case SerieType.Ring:
                        PropertyTwoFiled(prop, "m_Center");
                        PropertyTwoFiled(prop, "m_Radius");
                        PropertyField(prop, "m_StartAngle");
                        PropertyField(prop, "m_RingGap");
                        PropertyField(prop, "m_RoundCap");
                        PropertyField(prop, "m_Clockwise");
                        PropertyField(prop, "m_TitleStyle");
                        break;
                    case SerieType.Radar:
                        PropertyField(prop, "m_RadarType");
                        PropertyField(prop, "m_RadarIndex");
                        PropertyField(prop, "m_Symbol");
                        PropertyField(prop, "m_LineStyle");
                        PropertyField(prop, "m_AreaStyle");
                        break;
                    case SerieType.Scatter:
                    case SerieType.EffectScatter:
                        PropertyField(prop, "m_Clip");
                        PropertyField(prop, "m_Symbol");
                        break;
                    case SerieType.Heatmap:
                        PropertyField(prop, "m_Ignore");
                        PropertyField(prop, "m_IgnoreValue");
                        break;
                    case SerieType.Gauge:
                        PropertyField(prop, "m_GaugeType");
                        PropertyTwoFiled(prop, "m_Center");
                        PropertyTwoFiled(prop, "m_Radius");
                        PropertyField(prop, "m_Min");
                        PropertyField(prop, "m_Max");
                        PropertyField(prop, "m_StartAngle");
                        PropertyField(prop, "m_EndAngle");
                        PropertyFieldLimitMax(prop, "m_SplitNumber", 36);
                        PropertyField(prop, "m_RoundCap");
                        PropertyField(prop, "m_TitleStyle");
                        PropertyField(prop, "m_GaugeAxis");
                        PropertyField(prop, "m_GaugePointer");
                        break;
                    case SerieType.Liquid:
                        PropertyField(prop, "m_VesselIndex");
                        PropertyField(prop, "m_Min");
                        PropertyField(prop, "m_Max");
                        PropertyField(prop, "m_WaveLength");
                        PropertyField(prop, "m_WaveHeight");
                        PropertyField(prop, "m_WaveSpeed");
                        PropertyField(prop, "m_WaveOffset");
                        break;
                    case SerieType.Candlestick:
                        PropertyField(prop, "m_XAxisIndex");
                        PropertyField(prop, "m_YAxisIndex");
                        PropertyFieldLimitMin(prop, "m_MinShow", 0);
                        PropertyFieldLimitMin(prop, "m_MaxShow", 0);
                        PropertyFieldLimitMin(prop, "m_MaxCache", 0);
                        PropertyField(prop, "m_BarWidth");
                        PropertyField(prop, "m_Clip");
                        PropertyField(prop, "m_ShowAsPositiveNumber");
                        PropertyField(prop, "m_Large");
                        PropertyField(prop, "m_LargeThreshold");
                        break;
                    case SerieType.Custom:
                        var fileds = chart.GetCustomSerieInspectorShowFileds();
                        if (fileds != null && fileds.Length > 0)
                        {
                            foreach (var filed in fileds)
                            {
                                PropertyField(prop, filed);
                            }
                        }
                        var customs = chart.GetCustomSerieInspectorCustomFileds();
                        if (customs != null && customs.Length > 0)
                        {
                            foreach (var custom in customs)
                            {
                                var customProp = prop.FindPropertyRelative(custom[0]);
                                var anatherName = custom[1] + " (" + customProp.displayName + ")";
                                EditorGUI.PropertyField(m_DrawRect, customProp, new GUIContent(anatherName));
                                m_DrawRect.y += EditorGUI.GetPropertyHeight(prop);
                                m_Heights[m_KeyName] += hig;
                            }
                        }
                        break;
                }
                PropertyField(prop, "m_ItemStyle");
                PropertyField(prop, "m_IconStyle");
                PropertyField(prop, "m_Label");
                PropertyField(prop, "m_Emphasis");
                PropertyField(prop, "m_Animation");
                DrawData(pos, prop, serieType, ref m_DrawRect);
                --EditorGUI.indentLevel;
            }
        }

        private string[] GetChartSerieTypeNames(BaseChart chart)
        {
            var list = System.Enum.GetNames(typeof(SerieType));
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Equals("Custom"))
                {
                    var customName = chart.GetCustomSerieTypeName();
                    if (!string.IsNullOrEmpty(customName))
                    {
                        list[i] = customName;
                    }
                }
            }
            return list;
        }

        private void DrawData(Rect pos, SerializedProperty prop, SerieType serieType, ref Rect drawRect)
        {
            SerializedProperty m_Datas = prop.FindPropertyRelative("m_Data");
            SerializedProperty m_DataDimension = prop.FindPropertyRelative("m_ShowDataDimension");
            SerializedProperty m_ShowDataName = prop.FindPropertyRelative("m_ShowDataName");
            SerializedProperty m_ShowDataIcon = prop.FindPropertyRelative("m_ShowDataIcon");
            int index = InitToggle(prop);
            drawRect.width = EditorGUIUtility.labelWidth + 10;
            m_DataFoldout[index] = EditorGUI.Foldout(drawRect, m_DataFoldout[index], "Data", true);
            drawRect.width = pos.width;

            AddSingleLineHeight();
            if (m_DataFoldout[index])
            {
                EditorGUI.indentLevel++;

                float nameWid = 45;
#if UNITY_2019_3_OR_NEWER
                var gap = 2;
                var namegap = 3;
#else
                var gap = 0;
                var namegap = 0;
#endif
                EditorGUI.PropertyField(new Rect(drawRect.x, drawRect.y, pos.width - 2 * nameWid - 2, pos.height),
                    m_DataDimension);
                var nameRect = new Rect(pos.width - 2 * nameWid + 14 + gap, drawRect.y, nameWid - gap, pos.height);
                if (XChartsSettings.editorBlockEnable)
                {
                    nameRect.x += ChartEditorHelper.BLOCK_WIDTH;
                }
                if (GUI.Button(nameRect, new GUIContent("name")))
                {
                    m_ShowDataName.boolValue = !m_ShowDataName.boolValue;
                }
                var iconRect = new Rect(pos.width - nameWid + 14, drawRect.y, nameWid + namegap, pos.height);
                if (XChartsSettings.editorBlockEnable)
                {
                    iconRect.x += ChartEditorHelper.BLOCK_WIDTH;
                }
                if (GUI.Button(iconRect, new GUIContent("more")))
                {
                    m_ShowDataIcon.boolValue = !m_ShowDataIcon.boolValue;
                }
                var jsonRect = new Rect(pos.width - 70, drawRect.y - pos.height - 2, 90, pos.height);
                if (GUI.Button(jsonRect, new GUIContent("data from json")))
                {
                    PraseJsonDataEditor.chart = prop.serializedObject.targetObject as BaseChart;
                    PraseJsonDataEditor.serieIndex = index;
                    PraseJsonDataEditor.ShowWindow();
                }
                AddSingleLineHeight();
                var listSize = m_Datas.arraySize;
                listSize = EditorGUI.IntField(drawRect, "Size", listSize);
                AddSingleLineHeight();

                if (listSize < 0) listSize = 0;
                if (m_DataDimension.intValue < 1) m_DataDimension.intValue = 1;
                int dimension = m_DataDimension.intValue;
                bool showName = m_ShowDataName.boolValue;
                bool showIcon = m_ShowDataIcon.boolValue;
                bool showSelected = (serieType == SerieType.Pie);
                if (listSize != m_Datas.arraySize)
                {
                    while (listSize > m_Datas.arraySize) m_Datas.arraySize++;
                    while (listSize < m_Datas.arraySize) m_Datas.arraySize--;
                }
                if (listSize > 30 && !XChartsSettings.editorShowAllListData)
                {
                    int num = listSize > 10 ? 10 : listSize;
                    for (int i = 0; i < num; i++)
                    {
                        DrawDataElement(ref drawRect, dimension, m_Datas, showName, showIcon, showSelected, i, pos.width);
                    }
                    if (num >= 10)
                    {
                        EditorGUI.LabelField(drawRect, "...");
                        AddSingleLineHeight();
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
                AddHeight(EditorGUIUtility.standardVerticalSpacing);
                EditorGUI.indentLevel--;
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
                {
                    var value = data.arraySize == 0 ? index : 0;
                    data.arraySize++;
                    data.GetArrayElementAtIndex(data.arraySize - 1).floatValue = value;
                }
                SerializedProperty element = data.GetArrayElementAtIndex(1);
                if (showSelected)
                {
                    drawRect.width = drawRect.width - 18;
                    EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + index));
                    drawRect.x = currentWidth - 40;
                    EditorGUI.PropertyField(drawRect, selected, GUIContent.none);
                    drawRect.x = lastX;
                    drawRect.width = lastWid;
                }
                else
                {
                    EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + index));
                }
                AddHeight(EditorGUI.GetPropertyHeight(element));
            }
            else
            {
#if UNITY_2019_3_OR_NEWER
                var gap = 2;
                var namegap = 3;
#else
                var gap = 0;
                var namegap = 0;
#endif
                EditorGUI.LabelField(drawRect, "Element " + index);
                var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15 + gap;
                var dataWidTotal = (currentWidth - (startX + 20.5f + 1));
                var dataWid = dataWidTotal / fieldCount;
                var xWid = dataWid - 2;
                for (int i = 0; i < dimension; i++)
                {
                    var dataCount = i < 1 ? 2 : i + 1;
                    while (dataCount > data.arraySize)
                    {
                        var value = data.arraySize == 0 ? index : 0;
                        data.arraySize++;
                        data.GetArrayElementAtIndex(data.arraySize - 1).floatValue = value;
                    }
                    drawRect.x = startX + i * xWid;
                    drawRect.width = dataWid + 40;
                    SerializedProperty element = data.GetArrayElementAtIndex(dimension <= 1 ? 1 : i);
                    EditorGUI.PropertyField(drawRect, element, GUIContent.none);
                }
                if (showName)
                {
                    drawRect.x = startX + (fieldCount - 1) * xWid;
                    drawRect.width = dataWid + 40 + dimension * namegap;
                    EditorGUI.PropertyField(drawRect, sereName, GUIContent.none);
                }
                AddSingleLineHeight();
                drawRect.x = lastX;
                drawRect.width = lastWid;
                EditorGUIUtility.fieldWidth = lastFieldWid;
                EditorGUIUtility.labelWidth = lastLabelWid;
            }
            if (showDetail)
            {
                EditorGUI.indentLevel += 2;
                var m_Ignore = serieData.FindPropertyRelative("m_Ignore");
                var m_Selected = serieData.FindPropertyRelative("m_Selected");
                var m_Id = serieData.FindPropertyRelative("m_Id");
                var m_EnableIcon = serieData.FindPropertyRelative("m_EnableIconStyle");
                var m_Icon = serieData.FindPropertyRelative("m_IconStyle");
                var m_EnableLabel = serieData.FindPropertyRelative("m_EnableLabel");
                var m_Label = serieData.FindPropertyRelative("m_Label");
                var m_EnableItemStyle = serieData.FindPropertyRelative("m_EnableItemStyle");
                var m_ItemStyle = serieData.FindPropertyRelative("m_ItemStyle");
                var m_EnableEmphasis = serieData.FindPropertyRelative("m_EnableEmphasis");
                var m_Emphasis = serieData.FindPropertyRelative("m_Emphasis");
                var m_EnableSymbol = serieData.FindPropertyRelative("m_EnableSymbol");
                var m_Symbol = serieData.FindPropertyRelative("m_Symbol");
                EditorGUI.PropertyField(drawRect, m_Ignore);
                AddHeight(EditorGUI.GetPropertyHeight(m_Ignore));
                EditorGUI.PropertyField(drawRect, m_Selected);
                AddHeight(EditorGUI.GetPropertyHeight(m_Selected));
                EditorGUI.PropertyField(drawRect, m_Id);
                AddHeight(EditorGUI.GetPropertyHeight(m_Id));
                EditorGUI.PropertyField(drawRect, m_Icon);
                ChartEditorHelper.MakeBool(drawRect, m_EnableIcon, 1, "(enable)");
                AddHeight(EditorGUI.GetPropertyHeight(m_Icon));
                EditorGUI.PropertyField(drawRect, m_Symbol);
                ChartEditorHelper.MakeBool(drawRect, m_EnableSymbol, 1, "(enable)");
                AddHeight(EditorGUI.GetPropertyHeight(m_Symbol));
                EditorGUI.PropertyField(drawRect, m_Label);
                ChartEditorHelper.MakeBool(drawRect, m_EnableLabel, 1, "(enable)");
                AddHeight(EditorGUI.GetPropertyHeight(m_Label));
                EditorGUI.PropertyField(drawRect, m_ItemStyle);
                ChartEditorHelper.MakeBool(drawRect, m_EnableItemStyle, 1, "(enable)");
                AddHeight(EditorGUI.GetPropertyHeight(m_ItemStyle));
                EditorGUI.PropertyField(drawRect, m_Emphasis);
                ChartEditorHelper.MakeBool(drawRect, m_EnableEmphasis, 1, "(enable)");
                AddHeight(EditorGUI.GetPropertyHeight(m_Emphasis));
                EditorGUI.indentLevel -= 2;
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
            while (index >= m_DataFoldout.Count)
            {
                m_DataFoldout.Add(false);
            }
            return index;
        }
    }
}