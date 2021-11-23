/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace XCharts
{
    [Serializable]
    public class DebugInfo
    {
        [SerializeField] private bool m_Show;
        [SerializeField]
        private TextStyle m_TextStyle = new TextStyle()
        {
            fontSize = 16,
            backgroundColor = new Color32(32, 32, 32, 151),
            color = Color.white
        };

        private static StringBuilder s_Sb = new StringBuilder();

        private static readonly float INTERVAL = 0.2f;
        private static readonly float MAXCACHE = 20;
        private int m_FrameCount = 0;
        private float m_LastTime = 0f;
        private int m_LastRefreshCount = 0;
        private BaseChart m_Chart;
        private ChartLabel m_Label;
        private List<float> m_FpsList = new List<float>();


        public float fps { get; private set; }
        public float avgFps { get; private set; }
        public int refreshCount { get; internal set; }

        public void Init(BaseChart chart)
        {
            m_Chart = chart;
            m_Label = AddDebugInfoObject("debug", chart.transform, m_TextStyle, chart.theme);
        }

        public void Update()
        {
            if (!m_Show || m_Label == null) return;
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
                    s_Sb.AppendFormat("fps : {0:f0} / {1:f0}\n", fps, avgFps);
                    s_Sb.AppendFormat("data : {0}\n", m_Chart.GetAllSerieDataCount());
                    s_Sb.AppendFormat("refresh : {0}", refreshCount);
                    m_Label.SetText(s_Sb.ToString());
                }
            }
        }

        private float GetAvg(List<float> list)
        {
            var total = 0f;
            foreach (var v in list) total += v;
            return total / list.Count;
        }

        private ChartLabel AddDebugInfoObject(string name, Transform parent, TextStyle textStyle,
            ThemeStyle theme)
        {
            var anchorMax = new Vector2(0, 1);
            var anchorMin = new Vector2(0, 1);
            var pivot = new Vector2(0, 1);
            var sizeDelta = new Vector2(100, 100);

            var labelGameObject = ChartHelper.AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            var active = m_Chart.debugModel && m_Show;
            ChartHelper.SetActive(labelGameObject, active);
            if (!active)
            {
                return null;
            }
            var label = ChartHelper.GetOrAddComponent<ChartLabel>(labelGameObject);
            labelGameObject.hideFlags = m_Chart.chartHideFlags;
            label.labelBackground = label;
            label.labelBackground.color = textStyle.backgroundColor;
            label.labelBackground.raycastTarget = false;
            label.label = ChartHelper.AddTextObject("Text", label.gameObject.transform, anchorMin, anchorMax, pivot, sizeDelta, textStyle, theme.common);
            label.SetAutoSize(true);
            label.label.SetAlignment(textStyle.GetAlignment(TextAnchor.UpperLeft));
            label.label.SetLocalPosition(new Vector2(3, -3));
            label.SetText("30");
            label.SetTextColor(textStyle.color);
            return label;
        }
    }
}