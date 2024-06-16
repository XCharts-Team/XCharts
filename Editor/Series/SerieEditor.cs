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
        private bool m_LinksFoldout = false;
        private Dictionary<int, bool> m_LinksElementFoldout = new Dictionary<int, bool>();

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
            OnEndCustomInspectorGUI();
            --EditorGUI.indentLevel;
        }

        public virtual void OnCustomInspectorGUI()
        { }

        public virtual void OnEndCustomInspectorGUI()
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

        private HeaderMenuInfo headMenuInfo = new HeaderMenuInfo("Import ECharts Data", null);

        private void HeadMenuInfoCallback()
        {
            PraseExternalDataEditor.UpdateData(chart, serie, null, false);
            PraseExternalDataEditor.ShowWindow();
        }

        private void PropertyFieldData()
        {
            headMenuInfo.action = HeadMenuInfoCallback;
            m_DataFoldout = ChartEditorHelper.DrawHeader("Data", m_DataFoldout, false, null, null, headMenuInfo);
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

        private HeaderMenuInfo linkHeadMenuInfo = new HeaderMenuInfo("Import ECharts Link", null);

        private void LinkHeadMenuInfoCallback()
        {
            PraseExternalDataEditor.UpdateData(chart, serie, null, false);
            PraseExternalDataEditor.ShowWindow();
        }

        protected void PropertyFieldLinks()
        {
            linkHeadMenuInfo.action = LinkHeadMenuInfoCallback;
            m_LinksFoldout = ChartEditorHelper.DrawHeader("Links", m_LinksFoldout, false, null, null, linkHeadMenuInfo);
            if (!m_LinksFoldout) return;
            EditorGUI.indentLevel++;
            var m_Links = FindProperty("m_Links");
            var listSize = m_Links.arraySize;
            listSize = EditorGUILayout.IntField("Size", listSize);
            if (listSize < 0) listSize = 0;
            if (listSize != m_Links.arraySize)
            {
                while (listSize > m_Links.arraySize) m_Links.arraySize++;
                while (listSize < m_Links.arraySize) m_Links.arraySize--;
            }
            if (listSize > 30) // && !XCSettings.editorShowAllListData)
            {
                int num = listSize > 10 ? 10 : listSize;
                for (int i = 0; i < num; i++)
                {
                    DrawSerieDataLink(m_Links, i);
                }
                if (num >= 10)
                {
                    ChartEditorHelper.DrawHeader("... ", false, false, null, null);
                    DrawSerieDataLink(m_Links, listSize - 1);
                }
            }
            else
            {
                for (int i = 0; i < m_Links.arraySize; i++)
                {
                    DrawSerieDataLink(m_Links, i);
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

        private void DrawSerieDataHeader(Rect drawRect, HeaderCallbackContext context)
        {
            var serieData = context.serieData;
            var fieldCount = context.fieldCount;
            var showName = context.showName;
            var index = context.index;
            var dimension = context.dimension;

            //drawRect.width -= 2f;
            var maxX = drawRect.xMax;
            var currentWidth = drawRect.width;
            var lastX = drawRect.x;
            var lastWid = drawRect.width;
            var lastFieldWid = EditorGUIUtility.fieldWidth;
            var lastLabelWid = EditorGUIUtility.labelWidth;
            var sereName = serieData.FindPropertyRelative("m_Name");
            var data = serieData.FindPropertyRelative("m_Data");
#if UNITY_2019_3_OR_NEWER
            var gap = 2;
            var namegap = 3;
            var buttomLength = 30;
#else
            var gap = 0;
            var namegap = 0;
            var buttomLength = 30;
#endif
            if (showName)
            {
                buttomLength += 12;
            }
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
                drawRect.xMax = maxX - buttomLength;
                EditorGUI.PropertyField(drawRect, element, GUIContent.none);
            }
            else
            {
                var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15 + gap;
                var dataWidTotal = currentWidth - (startX + 20.5f + 1) - buttomLength;
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
                drawRect.x = lastX;
                drawRect.width = lastWid;
                ChartEditorHelper.UpDownAddDeleteButton(drawRect, context.listProp, index);
                EditorGUIUtility.fieldWidth = lastFieldWid;
                EditorGUIUtility.labelWidth = lastLabelWid;
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
            var callbackContext = new HeaderCallbackContext()
            {
                serieData = serieData,
                fieldCount = fieldCount,
                showName = showName,
                index = index,
                dimension = dimension,
                listProp = m_Datas
            };
            m_DataElementFoldout[index] = ChartEditorHelper.DrawSerieDataHeader("SerieData " + dataIndex, flag, false, null, callbackContext, DrawSerieDataHeader);
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

        private void DrawSerieDataLink(SerializedProperty m_Datas, int index)
        {
            bool flag;
            if (!m_LinksElementFoldout.TryGetValue(index, out flag))
            {
                flag = false;
                m_LinksElementFoldout[index] = false;
            }
            var dataLink = m_Datas.GetArrayElementAtIndex(index);
            m_LinksElementFoldout[index] = ChartEditorHelper.DrawHeader("Link " + index, flag, false, null,
                delegate (Rect drawRect)
                {
                    var sourceIndex = dataLink.FindPropertyRelative("m_Source");
                    var targetIndex = dataLink.FindPropertyRelative("m_Target");
                    var value = dataLink.FindPropertyRelative("m_Value");
                    var hig = ChartEditorHelper.MakeThreeField(ref drawRect, drawRect.width, sourceIndex, targetIndex, value, "");
                    var btnRect = drawRect;
                    btnRect.y -= hig;
                    ChartEditorHelper.UpDownAddDeleteButton(btnRect, m_Datas, index);
                });
            if (m_LinksElementFoldout[index])
            {
                DrawSerieDataLinkDetail(m_Datas, index);
            }
        }

        private void DrawSerieDataLinkDetail(SerializedProperty m_Links, int index)
        {
            EditorGUI.indentLevel++;
            var dataLink = m_Links.GetArrayElementAtIndex(index);
            PropertyField(dataLink.FindPropertyRelative("m_Source"));
            PropertyField(dataLink.FindPropertyRelative("m_Target"));
            PropertyField(dataLink.FindPropertyRelative("m_Value"));
            EditorGUI.indentLevel--;
        }
    }
}