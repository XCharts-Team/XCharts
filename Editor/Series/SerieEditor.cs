using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    public class SerieEditor<T> : SerieBaseEditor where T : Serie
    {
        protected const string MORE = "More";
        protected bool m_MoreFoldout = false;
        private bool m_DataFoldout = false;
        private bool m_DataComponentFoldout = true;
        private Dictionary<int, bool> m_DataElementFoldout = new Dictionary<int, bool>();

        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_SerieName");
            if (m_CoordOptionsNames != null && m_CoordOptionsNames.Count > 1)
            {
                var index = m_CoordOptionsNames.IndexOf(serie.coordSystem);
                var selectedIndex = EditorGUILayout.Popup("Coord System", index, m_CoordOptionsNames.ToArray());
                if (selectedIndex != index)
                {
                    var typeName = m_CoordOptionsNames[selectedIndex];
                    serie.coordSystem = m_CoordOptionsDic[typeName].Name;
                }
            }
            PropertyField("m_State");
            OnCustomInspectorGUI();
            OnExtraInspectorGUI();
            PropertyFieldData();
            --EditorGUI.indentLevel;
        }

        public virtual void OnCustomInspectorGUI()
        { }

        private void OnExtraInspectorGUI()
        {
            foreach (var kv in Serie.extraComponentMap)
            {
                var prop = FindProperty(kv.Value);
                if (prop.arraySize > 0)
                    PropertyField(prop.GetArrayElementAtIndex(0));
            }
        }

        private void PropertyFieldData()
        {
            m_DataFoldout = ChartEditorHelper.DrawHeader("Data", m_DataFoldout, false, null, null,
                new HeaderMenuInfo("Import ECharts Data", () =>
                {
                    PraseExternalDataEditor.UpdateData(chart, serie, null);
                    PraseExternalDataEditor.ShowWindow();
                }));
            if (!m_DataFoldout) return;
            EditorGUI.indentLevel++;
            var m_Datas = FindProperty("m_Data");
            var m_DataDimension = FindProperty("m_ShowDataDimension");
            var m_ShowDataName = FindProperty("m_ShowDataName");
            PropertyField(m_ShowDataName);
            PropertyField(m_DataDimension);
            var listSize = m_Datas.arraySize;
            listSize = EditorGUILayout.IntField("Size", listSize);
            if (listSize < 0) listSize = 0;
            if (m_DataDimension.intValue < 1) m_DataDimension.intValue = 1;
            int dimension = m_DataDimension.intValue;
            bool showName = m_ShowDataName.boolValue;
            if (listSize != m_Datas.arraySize)
            {
                while (listSize > m_Datas.arraySize) m_Datas.arraySize++;
                while (listSize < m_Datas.arraySize) m_Datas.arraySize--;
                serie.ResetDataIndex();
            }
            if (listSize > 30) // && !XCSettings.editorShowAllListData)
            {
                int num = listSize > 10 ? 10 : listSize;
                for (int i = 0; i < num; i++)
                {
                    DrawSerieData(dimension, m_Datas, i, showName);
                }
                if (num >= 10)
                {
                    ChartEditorHelper.DrawHeader("... ", false, false, null, null);
                    DrawSerieData(dimension, m_Datas, listSize - 1, showName);
                }
            }
            else
            {
                for (int i = 0; i < m_Datas.arraySize; i++)
                {
                    DrawSerieData(dimension, m_Datas, i, showName);
                }
            }
            EditorGUI.indentLevel--;
        }

        protected void PropertyFiledMore(System.Action action)
        {
            m_MoreFoldout = ChartEditorHelper.DrawHeader(MORE, m_MoreFoldout, false, null, null);
            if (m_MoreFoldout)
            {
                if (action != null) action();
            }
        }

        private void DrawSerieData(int dimension, SerializedProperty m_Datas, int index, bool showName)
        {
            bool flag;
            if (!m_DataElementFoldout.TryGetValue(index, out flag))
            {
                flag = false;
                m_DataElementFoldout[index] = false;
            }
            var fieldCount = dimension + (showName ? 1 : 0);
            var serieData = m_Datas.GetArrayElementAtIndex(index);
            var dataIndex = serieData.FindPropertyRelative("m_Index").intValue;
            m_DataElementFoldout[index] = ChartEditorHelper.DrawHeader("SerieData " + dataIndex, flag, false, null,
                delegate(Rect drawRect)
                {
                    //drawRect.width -= 2f;
                    var maxX = drawRect.xMax;
                    var currentWidth = drawRect.width;
                    var lastX = drawRect.x;
                    var lastWid = drawRect.width;
                    var lastFieldWid = EditorGUIUtility.fieldWidth;
                    var lastLabelWid = EditorGUIUtility.labelWidth;
                    //var serieData = m_Datas.GetArrayElementAtIndex(index);
                    var sereName = serieData.FindPropertyRelative("m_Name");
                    var data = serieData.FindPropertyRelative("m_Data");
#if UNITY_2019_3_OR_NEWER
                    var gap = 2;
                    var namegap = 3;
#else
                    var gap = 0;
                    var namegap = 0;
#endif
                    if (fieldCount <= 1)
                    {
                        while (2 > data.arraySize)
                        {
                            var value = data.arraySize == 0 ? index : 0;
                            data.arraySize++;
                            data.GetArrayElementAtIndex(data.arraySize - 1).floatValue = value;
                        }
                        SerializedProperty element = data.GetArrayElementAtIndex(1);
                        var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15 + gap;
                        drawRect.x = startX;
                        drawRect.xMax = maxX;
                        EditorGUI.PropertyField(drawRect, element, GUIContent.none);
                    }
                    else
                    {
                        var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15 + gap;
                        var dataWidTotal = (currentWidth - (startX + 20.5f + 1));
                        var dataWid = dataWidTotal / fieldCount;
                        var xWid = dataWid - 0;
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
                            drawRect.width = dataWid + 25;
                            SerializedProperty element = data.GetArrayElementAtIndex(dimension <= 1 ? 1 : i);
                            EditorGUI.PropertyField(drawRect, element, GUIContent.none);
                        }
                        if (showName)
                        {
                            drawRect.x = startX + (fieldCount - 1) * xWid;
                            drawRect.width = dataWid + 40 + dimension * namegap - 2.5f;
                            EditorGUI.PropertyField(drawRect, sereName, GUIContent.none);
                        }
                        EditorGUIUtility.fieldWidth = lastFieldWid;
                        EditorGUIUtility.labelWidth = lastLabelWid;
                    }
                });
            if (m_DataElementFoldout[index])
            {
                if (!(serie is ISimplifiedSerie))
                    DrawSerieDataDetail(m_Datas, index);
            }
        }

        private void DrawSerieDataDetail(SerializedProperty m_Datas, int index)
        {
            EditorGUI.indentLevel++;
            var serieData = m_Datas.GetArrayElementAtIndex(index);
            PropertyField(serieData.FindPropertyRelative("m_Name"));
            //PropertyField(serieData.FindPropertyRelative("m_State"));
            if (serie.GetType().IsDefined(typeof(SerieDataExtraFieldAttribute), false))
            {
                var attribute = serie.GetType().GetAttribute<SerieDataExtraFieldAttribute>();
                foreach (var field in attribute.fields)
                {
                    PropertyField(serieData.FindPropertyRelative(field));
                }
            }

            serieDataMenus.Clear();
            if (serie.GetType().IsDefined(typeof(SerieDataComponentAttribute), false))
            {
                var attribute = serie.GetType().GetAttribute<SerieDataComponentAttribute>();
                foreach (var type in attribute.types)
                {
                    var size = serieData.FindPropertyRelative(SerieData.extraComponentMap[type]).arraySize;
                    serieDataMenus.Add(new HeaderMenuInfo("Add " + type.Name, () =>
                    {
                        serie.GetSerieData(index).EnsureComponent(type);
                        EditorUtility.SetDirty(chart);
                    }, size == 0));
                }
                foreach (var type in attribute.types)
                {
                    var size = serieData.FindPropertyRelative(SerieData.extraComponentMap[type]).arraySize;
                    serieDataMenus.Add(new HeaderMenuInfo("Remove " + type.Name, () =>
                    {
                        serie.GetSerieData(index).RemoveComponent(type);
                        EditorUtility.SetDirty(chart);
                    }, size > 0));
                }
            }
            serieDataMenus.Add(new HeaderMenuInfo("Remove All", () =>
            {
                serie.GetSerieData(index).RemoveAllComponent();
            }, true));
            m_DataComponentFoldout = ChartEditorHelper.DrawHeader("Component", m_DataComponentFoldout, false, null, null, serieDataMenus);
            if (m_DataComponentFoldout)
            {
                foreach (var kv in SerieData.extraComponentMap)
                {
                    var prop = serieData.FindPropertyRelative(kv.Value);
                    if (prop.arraySize > 0)
                        PropertyField(prop.GetArrayElementAtIndex(0));
                }
            }
            EditorGUI.indentLevel--;
        }
    }
}