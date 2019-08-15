using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/RadarChart", 16)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class RadarChart : BaseChart
    {
        private const string INDICATOR_TEXT = "indicator";

        //[SerializeField] private Radar radar = Radar.defaultRadar;
        [SerializeField] private List<Radar> m_Radars = new List<Radar>();
        private List<Radar> m_CheckRadars = new List<Radar>();
        private bool m_IsEnterLegendButtom;

        //public Radar radar { get { return radar; } }
        public List<Radar> radars { get { return m_Radars; } }

        /// <summary>
        /// 移除所有数据，包含指示器数据。
        /// </summary>
        public override void RemoveData()
        {
            base.RemoveData();
            foreach (var radar in m_Radars)
            {
                radar.indicatorList.Clear();
            }
            m_CheckRadars.Clear();
        }

        protected override void OnLegendButtonClick(int index, string legendName, bool show)
        {
            bool active = CheckDataShow(legendName, show);
            var bgColor1 = active ? m_ThemeInfo.GetColor(index) : m_ThemeInfo.legendUnableColor;
            m_Legend.UpdateButtonColor(legendName, bgColor1);
            RefreshChart();
        }

        protected override void OnLegendButtonEnter(int index, string legendName)
        {
            m_IsEnterLegendButtom = true;
            CheckDataHighlighted(legendName, true);
            RefreshChart();
        }

        protected override void OnLegendButtonExit(int index, string legendName)
        {
            m_IsEnterLegendButtom = false;
            CheckDataHighlighted(legendName, false);
            RefreshChart();
        }

        protected override void Awake()
        {
            base.Awake();
            InitIndicator();
        }

        protected override void Update()
        {
            base.Update();
            CheckRadarInfoChanged();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            RemoveData();
            m_Radars.Add(Radar.defaultRadar);
            m_Title.text = "RadarChart";
            var serie = AddSerie("serie1", SerieType.Radar);
            serie.symbol.type = SerieSymbolType.EmptyCircle;
            serie.symbol.size = 4;
            serie.symbol.selectedSize = 6;
            List<float> data = new List<float>();
            for (int i = 0; i < 5; i++)
            {
                data.Add(Random.Range(20, 90));
            }
            AddData(0, data);
        }
#endif

        private void InitIndicator()
        {
            ChartHelper.HideAllObject(transform, INDICATOR_TEXT);
            for (int n = 0; n < m_Radars.Count; n++)
            {
                Radar radar = m_Radars[n];
                radar.UpdateRadarCenter(chartWidth, chartHeight);
                int indicatorNum = radar.indicatorList.Count;
                float txtWid = 100;
                float txtHig = 20;
                for (int i = 0; i < indicatorNum; i++)
                {
                    var indicator = radar.indicatorList[i];
                    var pos = radar.GetIndicatorPosition(i);
                    TextAnchor anchor = TextAnchor.MiddleCenter;
                    var diff = pos.x - radar.centerPos.x;
                    if (diff < -1f)
                    {
                        pos = new Vector3(pos.x - 5, pos.y);
                        anchor = TextAnchor.MiddleRight;
                    }
                    else if (diff > 1f)
                    {
                        anchor = TextAnchor.MiddleLeft;
                        pos = new Vector3(pos.x + txtWid + 5, pos.y);
                    }
                    else
                    {
                        anchor = TextAnchor.MiddleCenter;
                        float y = pos.y > radar.centerPos.y ? pos.y + txtHig / 2 : pos.y - txtHig / 2;
                        pos = new Vector3(pos.x + txtWid / 2, y);
                    }
                    var textColor = indicator.color == Color.clear ? (Color)m_ThemeInfo.axisTextColor : indicator.color;
                    Text txt = ChartHelper.AddTextObject(INDICATOR_TEXT + "_" + n + "_" + i, transform, m_ThemeInfo.font,
                        textColor, anchor, Vector2.zero, Vector2.zero, new Vector2(1, 0.5f),
                        new Vector2(txtWid, txtHig));
                    txt.transform.localPosition = pos;
                    txt.text = radar.indicatorList[i].name;
                    txt.gameObject.SetActive(radar.indicator);
                }
            }
        }

        private void CheckRadarInfoChanged()
        {
            if (!ChartHelper.IsValueEqualsList(m_CheckRadars, m_Radars))
            {
                m_CheckRadars.Clear();
                foreach (var radar in m_Radars) m_CheckRadars.Add(radar.Clone());
                OnRadarChanged();
            }
        }

        private void OnRadarChanged()
        {
            InitIndicator();
            m_Tooltip.UpdateToTop();
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            foreach (var radar in m_Radars)
            {
                radar.UpdateRadarCenter(chartWidth, chartHeight);
                if (radar.shape == Radar.Shape.Circle)
                {
                    DrawCricleRadar(vh, radar);
                }
                else
                {
                    DrawRadar(vh, radar);
                }
            }
            DrawData(vh);
        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
            foreach (var radar in m_Radars)
            {
                radar.splitArea.color.Clear();
                switch (m_ThemeInfo.theme)
                {
                    case Theme.Dark:
                        radar.splitArea.color.Add(ThemeInfo.GetColor("#6f6f6f"));
                        radar.splitArea.color.Add(ThemeInfo.GetColor("#606060"));
                        break;
                    case Theme.Default:
                        radar.splitArea.color.Add(ThemeInfo.GetColor("#f6f6f6"));
                        radar.splitArea.color.Add(ThemeInfo.GetColor("#e7e7e7"));
                        break;
                    case Theme.Light:
                        radar.splitArea.color.Add(ThemeInfo.GetColor("#f6f6f6"));
                        radar.splitArea.color.Add(ThemeInfo.GetColor("#e7e7e7"));
                        break;
                }
            }
            InitIndicator();
        }

        Dictionary<string, int> serieNameSet = new Dictionary<string, int>();
        private void DrawData(VertexHelper vh)
        {
            Vector3 startPoint = Vector3.zero;
            Vector3 toPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;

            serieNameSet.Clear();
            int serieNameCount = -1;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.series[i];
                var radar = m_Radars[serie.radarIndex];
                int indicatorNum = radar.indicatorList.Count;
                var angle = 2 * Mathf.PI / indicatorNum;
                Vector3 p = radar.centerPos;
                if (!IsActive(i))
                {
                    continue;
                }
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    int key = i * 100 + j;
                    if (!radar.dataPosList.ContainsKey(key))
                    {
                        radar.dataPosList.Add(i * 100 + j, new List<Vector3>(serieData.data.Count));
                    }
                    else
                    {
                        radar.dataPosList[key].Clear();
                    }
                    string dataName = serieData.name;
                    int serieIndex = 0;
                    if (string.IsNullOrEmpty(dataName))
                    {
                        serieNameCount++;
                        serieIndex = serieNameCount;
                    }
                    else if (!serieNameSet.ContainsKey(dataName))
                    {
                        serieNameSet.Add(dataName, serieNameCount);
                        serieNameCount++;
                        serieIndex = serieNameCount;
                    }
                    else
                    {
                        serieIndex = serieNameSet[dataName];
                    }
                    if (!serieData.show)
                    {
                        continue;
                    }
                    var isHighlight = serie.highlighted || serieData.highlighted ||
                        (m_Tooltip.show && m_Tooltip.dataIndex[0] == i && m_Tooltip.dataIndex[1] == j);
                    var areaColor = serie.GetAreaColor(m_ThemeInfo, serieIndex, isHighlight);
                    var lineColor = serie.GetLineColor(m_ThemeInfo, serieIndex, isHighlight);
                    int dataCount = radar.indicatorList.Count;
                    List<Vector3> pointList = radar.dataPosList[key];
                    for (int n = 0; n < dataCount; n++)
                    {
                        if (n >= serieData.data.Count) break;
                        float min = radar.GetIndicatorMin(n);
                        float max = radar.GetIndicatorMax(n);
                        float value = serieData.data[n];
                        if (max == 0)
                        {
                            serie.GetMinMaxData(n, out min, out max);
                            min = radar.GetIndicatorMin(n);
                        }
                        var radius = max < 0 ? radar.actualRadius - radar.actualRadius * value / max
                        : radar.actualRadius * value / max;
                        var currAngle = n * angle;
                        if (n == 0)
                        {
                            startPoint = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                                p.y + radius * Mathf.Cos(currAngle));
                            firstPoint = startPoint;
                        }
                        else
                        {
                            toPoint = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                                p.y + radius * Mathf.Cos(currAngle));
                            if (serie.areaStyle.show)
                            {
                                ChartHelper.DrawTriangle(vh, p, startPoint, toPoint, areaColor);
                            }
                            if (serie.lineStyle.show)
                            {
                                ChartHelper.DrawLine(vh, startPoint, toPoint, serie.lineStyle.width, lineColor);
                            }
                            startPoint = toPoint;
                        }
                        pointList.Add(startPoint);
                    }
                    if (serie.areaStyle.show)
                    {
                        ChartHelper.DrawTriangle(vh, p, startPoint, firstPoint, areaColor);
                    }
                    if (serie.lineStyle.show)
                    {
                        ChartHelper.DrawLine(vh, startPoint, firstPoint, serie.lineStyle.width, lineColor);
                    }
                    if (serie.symbol.type != SerieSymbolType.None)
                    {
                        var symbolSize = (isHighlight ? serie.symbol.selectedSize : serie.symbol.size);
                        float symbolRadius = symbolSize - serie.lineStyle.width * 2;
                        var symbolColor = serie.symbol.color != Color.clear ? serie.symbol.color : lineColor;
                        symbolColor.a *= serie.symbol.opacity;
                        foreach (var point in pointList)
                        {
                            DrawSymbol(vh, serie.symbol.type, symbolSize, serie.lineStyle.width, point, symbolColor);
                        }
                    }
                }
            }
        }

        private void DrawRadar(VertexHelper vh, Radar radar)
        {
            if (!radar.lineStyle.show && !radar.splitArea.show)
            {
                return;
            }
            float insideRadius = 0, outsideRadius = 0;
            float block = radar.actualRadius / radar.splitNumber;
            int indicatorNum = radar.indicatorList.Count;
            Vector3 p1, p2, p3, p4;
            Vector3 p = radar.centerPos;
            float angle = 2 * Mathf.PI / indicatorNum;
            var lineColor = GetLineColor(radar);
            for (int i = 0; i < radar.splitNumber; i++)
            {
                Color color = radar.splitArea.color[i % radar.splitArea.color.Count];
                outsideRadius = insideRadius + block;
                p1 = new Vector3(p.x + insideRadius * Mathf.Sin(0), p.y + insideRadius * Mathf.Cos(0));
                p2 = new Vector3(p.x + outsideRadius * Mathf.Sin(0), p.y + outsideRadius * Mathf.Cos(0));
                for (int j = 0; j <= indicatorNum; j++)
                {
                    float currAngle = j * angle;
                    p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                        p.y + outsideRadius * Mathf.Cos(currAngle));
                    p4 = new Vector3(p.x + insideRadius * Mathf.Sin(currAngle),
                        p.y + insideRadius * Mathf.Cos(currAngle));
                    if (radar.splitArea.show)
                    {
                        ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, color);
                    }
                    if (radar.lineStyle.show)
                    {
                        ChartHelper.DrawLine(vh, p2, p3, radar.lineStyle.width, lineColor);
                    }
                    p1 = p4;
                    p2 = p3;
                }
                insideRadius = outsideRadius;
            }
            for (int j = 0; j <= indicatorNum; j++)
            {
                float currAngle = j * angle;
                p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                    p.y + outsideRadius * Mathf.Cos(currAngle));
                if (radar.lineStyle.show)
                {
                    ChartHelper.DrawLine(vh, p, p3, radar.lineStyle.width / 2, lineColor);
                }
            }
        }

        private void DrawCricleRadar(VertexHelper vh, Radar radar)
        {
            if (!radar.lineStyle.show && !radar.splitArea.show)
            {
                return;
            }
            float insideRadius = 0, outsideRadius = 0;
            float block = radar.actualRadius / radar.splitNumber;
            int indicatorNum = radar.indicatorList.Count;
            Vector3 p = radar.centerPos;
            Vector3 p1;
            float angle = 2 * Mathf.PI / indicatorNum;
            var lineColor = GetLineColor(radar);
            for (int i = 0; i < radar.splitNumber; i++)
            {
                Color color = radar.splitArea.color[i % radar.splitArea.color.Count];
                outsideRadius = insideRadius + block;
                if (radar.splitArea.show)
                {
                    ChartHelper.DrawDoughnut(vh, p, insideRadius, outsideRadius, 0, 360, color);
                }
                if (radar.lineStyle.show)
                {
                    ChartHelper.DrawCicleNotFill(vh, p, outsideRadius, radar.lineStyle.width, lineColor);
                }
                insideRadius = outsideRadius;
            }
            for (int j = 0; j <= indicatorNum; j++)
            {
                float currAngle = j * angle;
                p1 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                    p.y + outsideRadius * Mathf.Cos(currAngle));
                if (radar.lineStyle.show)
                {
                    ChartHelper.DrawLine(vh, p, p1, radar.lineStyle.width / 2, lineColor);
                }
            }
        }

        private Color GetLineColor(Radar radar)
        {
            if (radar.lineStyle.color != Color.clear)
            {
                var color = radar.lineStyle.color;
                color.a *= radar.lineStyle.opacity;
                return color;
            }
            else
            {
                var color = (Color)m_ThemeInfo.axisLineColor;
                color.a *= radar.lineStyle.opacity;
                return color;
            }
        }


        protected override void CheckTootipArea(Vector2 local)
        {
            if (m_IsEnterLegendButtom) return;

            bool highlight = false;
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (!IsActive(i)) continue;
                var serie = m_Series.GetSerie(i);
                var radar = m_Radars[serie.radarIndex];
                var dist = Vector2.Distance(radar.centerPos, local);
                if (dist > radar.actualRadius + serie.symbol.size)
                {
                    continue;
                }
                for (int n = 0; n < serie.data.Count; n++)
                {
                    var posKey = i * 100 + n;
                    if (radar.dataPosList.ContainsKey(posKey))
                    {
                        var posList = radar.dataPosList[posKey];
                        foreach (var pos in posList)
                        {
                            if (Vector2.Distance(pos, local) <= serie.symbol.size * 1.2f)
                            {
                                m_Tooltip.dataIndex[0] = i;
                                m_Tooltip.dataIndex[1] = n;
                                highlight = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (!highlight)
            {
                if (m_Tooltip.IsActive())
                {
                    m_Tooltip.SetActive(false);
                    RefreshChart();
                }
            }
            else
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                RefreshTooltip();
                RefreshChart();
            }
        }

        protected override void RefreshTooltip()
        {
            base.RefreshTooltip();
            int serieIndex = m_Tooltip.dataIndex[0];
            if (serieIndex < 0)
            {
                m_Tooltip.SetActive(false);
                return;
            }
            m_Tooltip.SetActive(true);
            var serie = m_Series.GetSerie(serieIndex);
            var radar = m_Radars[serie.radarIndex];
            var serieData = serie.GetSerieData(m_Tooltip.dataIndex[1]);
            StringBuilder sb = new StringBuilder(serieData.name);
            for (int i = 0; i < radar.indicatorList.Count; i++)
            {
                string key = radar.indicatorList[i].name;
                float value = serieData.GetData(i);
                sb.Append("\n");
                sb.AppendFormat("{0}: {1}", key, value);
            }
            m_Tooltip.UpdateContentText(sb.ToString());

            var pos = m_Tooltip.GetContentPos();
            if (pos.x + m_Tooltip.width > chartWidth)
            {
                pos.x = chartWidth - m_Tooltip.width;
            }
            if (pos.y - m_Tooltip.height < 0)
            {
                pos.y = m_Tooltip.height;
            }
            m_Tooltip.UpdateContentPos(pos);
        }
    }
}
