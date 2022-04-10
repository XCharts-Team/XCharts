/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

namespace XCharts
{
    [Serializable]
    public class DebugInfo
    {
        private bool m_ShowDebugInfo = false;
        private TextStyle m_DebugInfoTextStyle = new TextStyle()
        {
            fontSize = 18,
            backgroundColor = new Color32(32, 32, 32, 170),
            color = Color.white
        };

        private static StringBuilder s_Sb = new StringBuilder();

        private static readonly float INTERVAL = 0.2f;
        private static readonly float MAXCACHE = 20;
        private int m_FrameCount = 0;
        private float m_LastTime = 0f;
        private float m_LastCheckShowTime = 0f;
        private int m_LastRefreshCount = 0;
        private BaseChart m_Chart;
        private ChartLabel m_Label;
        private List<float> m_FpsList = new List<float>();
        public float fps { get; private set; }
        public float avgFps { get; private set; }
        public int refreshCount { get; internal set; }
        internal int clickChartCount { get; set; }

        public void Init(BaseChart chart)
        {
            m_Chart = chart;
            m_Label = AddDebugInfoObject("debug", chart.transform, m_DebugInfoTextStyle, chart.theme);
        }

        public void Update()
        {
            if (clickChartCount >= 3)
            {
                m_ShowDebugInfo = !m_ShowDebugInfo;
                ChartHelper.SetActive(m_Label.gameObject.transform, m_ShowDebugInfo);
                clickChartCount = 0;
                m_LastCheckShowTime = Time.realtimeSinceStartup;
                return;
            }
            if (Time.realtimeSinceStartup - m_LastCheckShowTime > 0.5f)
            {
                m_LastCheckShowTime = Time.realtimeSinceStartup;
                clickChartCount = 0;
            }
            if (!m_ShowDebugInfo || m_Label == null)
                return;

            m_FrameCount++;
            if (Time.realtimeSinceStartup - m_LastTime >= INTERVAL)
            {
                fps = m_FrameCount / (Time.realtimeSinceStartup - m_LastTime);
                m_FrameCount = 0;
                m_LastTime = Time.realtimeSinceStartup;
                if (m_LastRefreshCount == refreshCount)
                {
                    m_LastRefreshCount = 0;
                    refreshCount = 0;
                }
                m_LastRefreshCount = refreshCount;
                if (m_FpsList.Count > MAXCACHE)
                {
                    m_FpsList.RemoveAt(0);
                }
                m_FpsList.Add(fps);
                avgFps = GetAvg(m_FpsList);
                if (m_Label != null)
                {
                    s_Sb.Length = 0;
                    s_Sb.AppendFormat("v{0}\n", XChartsMgr.version);
                    s_Sb.AppendFormat("fps : {0:f0} / {1:f0}\n", fps, avgFps);
                    s_Sb.AppendFormat("draw : {0}\n", refreshCount);

                    var dataCount = m_Chart.series.GetAllSerieDataCount();
                    SetValueWithKInfo(s_Sb, "data", dataCount);

                    var vertCount = 0;
                    foreach (var serie in m_Chart.series.list)
                        vertCount += serie.runtimeVertCount;

                    SetValueWithKInfo(s_Sb, "b-vert", m_Chart.m_BasePainterVertCount);
                    SetValueWithKInfo(s_Sb, "s-vert", vertCount);
                    SetValueWithKInfo(s_Sb, "t-vert", m_Chart.m_TopPainterVertCount, false);

                    m_Label.SetText(s_Sb.ToString());
                }
            }
        }

        private static void SetValueWithKInfo(StringBuilder s_Sb, string key, int value, bool newLine = true)
        {
            if (value >= 1000)
                s_Sb.AppendFormat("{0} : {1:f1}k", key, value * 0.001f);
            else
                s_Sb.AppendFormat("{0} : {1}", key, value);
            if (newLine)
                s_Sb.Append("\n");
        }

        private static float GetAvg(List<float> list)
        {
            var total = 0f;
            foreach (var v in list) total += v;
            return total / list.Count;
        }

        private ChartLabel AddDebugInfoObject(string name, Transform parent, TextStyle textStyle,
            ChartTheme theme)
        {
            var anchorMax = new Vector2(0, 1);
            var anchorMin = new Vector2(0, 1);
            var pivot = new Vector2(0, 1);
            var sizeDelta = new Vector2(130, 150);

            var labelGameObject = ChartHelper.AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            labelGameObject.transform.SetAsLastSibling();
            labelGameObject.hideFlags = m_Chart.chartHideFlags;
            ChartHelper.SetActive(labelGameObject, m_ShowDebugInfo);

            var image = ChartHelper.GetOrAddComponent<Image>(labelGameObject);
            image.color = textStyle.backgroundColor;

            var label = new ChartLabel();
            label.gameObject = labelGameObject;
            label.label = ChartHelper.AddTextObject("Text", label.gameObject.transform, anchorMin, anchorMax,
                pivot, sizeDelta, textStyle, theme.common);
            label.SetAutoSize(true);
            label.label.SetActive(true);
            label.label.SetAlignment(textStyle.GetAlignment(TextAnchor.UpperLeft));
            label.label.SetLocalPosition(new Vector2(3, -3));
            label.SetText("30");
            label.SetLabelColor(textStyle.color);
            return label;
        }
    }
}