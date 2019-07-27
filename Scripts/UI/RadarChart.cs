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

        [SerializeField]
        private Radar m_Radar = Radar.defaultRadar;
        private Radar m_CheckRadar = Radar.defaultRadar;
        private float m_RadarCenterX = 0f;
        private float m_RadarCenterY = 0f;
        private float m_RadarRadius = 0;
        private List<Text> indicatorTextList = new List<Text>();
        private List<List<Vector3>> dataPosList = new List<List<Vector3>>();

        public Radar radar { get { return m_Radar; } }

        public override void RemoveData()
        {
            base.RemoveData();
            m_Radar.indicatorList.Clear();
        }
        protected override void Awake()
        {
            base.Awake();
            UpdateRadarCenter();
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
            m_Radar = Radar.defaultRadar;
            m_Title.text = "RadarChart";
            AddSerie("serie1", SerieType.Radar);
            for (int i = 0; i < 5; i++)
            {
                AddData(0, Random.Range(20, 90));
            }
        }
#endif


        private void InitIndicator()
        {
            indicatorTextList.Clear();
            ChartHelper.HideAllObject(transform, INDICATOR_TEXT);
            int indicatorNum = m_Radar.indicatorList.Count;
            float txtWid = 100;
            float txtHig = 20;
            for (int i = 0; i < indicatorNum; i++)
            {
                var pos = GetIndicatorPosition(i);
                TextAnchor anchor = TextAnchor.MiddleCenter;
                var diff = pos.x - m_RadarCenterX;
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
                    float y = pos.y > m_RadarCenterY ? pos.y + txtHig / 2 : pos.y - txtHig / 2;
                    pos = new Vector3(pos.x + txtWid / 2, y);
                }
                Text txt = ChartHelper.AddTextObject(INDICATOR_TEXT + i, transform, m_ThemeInfo.font,
                    m_ThemeInfo.textColor, anchor, Vector2.zero, Vector2.zero, new Vector2(1, 0.5f),
                    new Vector2(txtWid, txtHig));
                txt.transform.localPosition = pos;
                txt.text = m_Radar.indicatorList[i].name;
                txt.gameObject.SetActive(m_Radar.indicator);
                indicatorTextList.Add(txt);
            }
        }

        private void CheckRadarInfoChanged()
        {
            if (m_CheckRadar != m_Radar)
            {
                m_CheckRadar.Copy(m_Radar);
                OnRadarChanged();
            }
        }

        private void OnRadarChanged()
        {
            UpdateRadarCenter();
            InitIndicator();
            m_Tooltip.UpdateToTop();
        }

        private Vector3 GetIndicatorPosition(int i)
        {
            int indicatorNum = m_Radar.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum * i;
            var x = m_RadarCenterX + m_Radar.radius * Mathf.Sin(angle);
            var y = m_RadarCenterY + m_Radar.radius * Mathf.Cos(angle);

            return new Vector3(x, y);
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            UpdateRadarCenter();
            if (m_Radar.cricle)
                DrawCricleRadar(vh);
            else
                DrawRadar(vh);
            DrawData(vh);
        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
            m_Radar.backgroundColorList.Clear();
            switch (m_ThemeInfo.theme)
            {
                case Theme.Dark:
                    m_Radar.backgroundColorList.Add(ThemeInfo.GetColor("#6f6f6f"));
                    m_Radar.backgroundColorList.Add(ThemeInfo.GetColor("#606060"));
                    break;
                case Theme.Default:
                    m_Radar.backgroundColorList.Add(ThemeInfo.GetColor("#f6f6f6"));
                    m_Radar.backgroundColorList.Add(ThemeInfo.GetColor("#e7e7e7"));
                    break;
                case Theme.Light:
                    m_Radar.backgroundColorList.Add(ThemeInfo.GetColor("#f6f6f6"));
                    m_Radar.backgroundColorList.Add(ThemeInfo.GetColor("#e7e7e7"));
                    break;
            }
            InitIndicator();
        }

HashSet<string> serieNameSet = new HashSet<string>();
        private void DrawData(VertexHelper vh)
        {
            int indicatorNum = m_Radar.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum;
            var p = new Vector3(m_RadarCenterX, m_RadarCenterY);
            Vector3 startPoint = Vector3.zero;
            Vector3 toPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;
            dataPosList.Clear();
            dataPosList.Capacity = m_Series.Count;
            serieNameSet.Clear();
            int serieNameCount = -1;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.series[i];
                if (string.IsNullOrEmpty(serie.name)) serieNameCount++;
                else if (!serieNameSet.Contains(serie.name))
                {
                    serieNameSet.Add(serie.name);
                    serieNameCount++;
                }
                if (!IsActive(i))
                {
                    dataPosList.Add(new List<Vector3>());
                    continue;
                }
                var dataList = m_Series.series[i].yData;
                var color = m_ThemeInfo.GetColor(i);
                var areaColor = color;
                areaColor.a = (byte)m_Radar.areaAipha;

                List<Vector3> pointList = new List<Vector3>(dataList.Count);
                dataPosList.Add(pointList);
                for (int j = 0; j < dataList.Count; j++)
                {
                    var max = m_Radar.GetIndicatorMax(j) > 0 ?
                        m_Radar.GetIndicatorMax(j) :
                        GetMaxValue(j);
                    var radius = max < 0 ? m_Radar.radius - m_Radar.radius * dataList[j] / max
                        : m_Radar.radius * dataList[j] / max;
                    var currAngle = j * angle;
                    if (j == 0)
                    {
                        startPoint = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                            p.y + radius * Mathf.Cos(currAngle));
                        firstPoint = startPoint;
                    }
                    else
                    {
                        toPoint = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                            p.y + radius * Mathf.Cos(currAngle));
                        if (m_Radar.area)
                        {
                            ChartHelper.DrawTriangle(vh, p, startPoint, toPoint, areaColor);
                        }
                        ChartHelper.DrawLine(vh, startPoint, toPoint, m_Radar.lineTickness, color);
                        startPoint = toPoint;
                    }
                    pointList.Add(startPoint);
                }
                if (m_Radar.area) ChartHelper.DrawTriangle(vh, p, startPoint, firstPoint, areaColor);
                ChartHelper.DrawLine(vh, startPoint, firstPoint, m_Radar.lineTickness, color);
                foreach (var point in pointList)
                {
                    float radius = m_Radar.linePointSize - m_Radar.lineTickness * 2;

                    ChartHelper.DrawCricle(vh, point, radius, Color.white);
                    ChartHelper.DrawDoughnut(vh, point, radius, m_Radar.linePointSize, 0, 360, color);
                }
            }
        }

        private void DrawRadar(VertexHelper vh)
        {
            float insideRadius = 0, outsideRadius = 0;
            float block = m_Radar.radius / m_Radar.splitNumber;
            int indicatorNum = m_Radar.indicatorList.Count;
            Vector3 p1, p2, p3, p4;
            Vector3 p = new Vector3(m_RadarCenterX, m_RadarCenterY);
            float angle = 2 * Mathf.PI / indicatorNum;
            for (int i = 0; i < m_Radar.splitNumber; i++)
            {
                Color color = m_Radar.backgroundColorList[i % m_Radar.backgroundColorList.Count];
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

                    ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, color);
                    ChartHelper.DrawLine(vh, p2, p3, m_Radar.lineTickness, m_Radar.lineColor);
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
                ChartHelper.DrawLine(vh, p, p3, m_Radar.lineTickness / 2, m_Radar.lineColor);
            }
        }

        private void DrawCricleRadar(VertexHelper vh)
        {
            float insideRadius = 0, outsideRadius = 0;
            float block = m_Radar.radius / m_Radar.splitNumber;
            int indicatorNum = m_Radar.indicatorList.Count;
            Vector3 p = new Vector3(m_RadarCenterX, m_RadarCenterY);
            Vector3 p1;
            float angle = 2 * Mathf.PI / indicatorNum;
            for (int i = 0; i < m_Radar.splitNumber; i++)
            {
                Color color = m_Radar.backgroundColorList[i % m_Radar.backgroundColorList.Count];
                outsideRadius = insideRadius + block;
                ChartHelper.DrawDoughnut(vh, p, insideRadius, outsideRadius, 0, 360, color);
                ChartHelper.DrawCicleNotFill(vh, p, outsideRadius, m_Radar.lineTickness, m_Radar.lineColor);
                insideRadius = outsideRadius;
            }
            for (int j = 0; j <= indicatorNum; j++)
            {
                float currAngle = j * angle;
                p1 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                    p.y + outsideRadius * Mathf.Cos(currAngle));
                ChartHelper.DrawLine(vh, p, p1, m_Radar.lineTickness / 2, m_Radar.lineColor);
            }
        }

        private void UpdateRadarCenter()
        {
            float diffX = chartWidth - m_Radar.left - m_Radar.right;
            float diffY = chartHeight - m_Radar.top - m_Radar.bottom;
            float diff = Mathf.Min(diffX, diffY);
            if (m_Radar.radius <= 0)
            {
                m_RadarRadius = diff / 3 * 2;
                m_RadarCenterX = m_Radar.left + m_RadarRadius;
                m_RadarCenterY = m_Radar.bottom + m_RadarRadius;
            }
            else
            {
                m_RadarRadius = m_Radar.radius;
                m_RadarCenterX = chartWidth / 2;
                m_RadarCenterY = chartHeight / 2;
                if (m_Radar.left > 0) m_RadarCenterX = m_Radar.left + m_RadarRadius;
                if (m_Radar.right > 0) m_RadarCenterX = chartWidth - m_Radar.right - m_RadarRadius;
                if (m_Radar.top > 0) m_RadarCenterY = chartHeight - m_Radar.top - m_RadarRadius;
                if (m_Radar.bottom > 0) m_RadarCenterY = m_Radar.bottom + m_RadarRadius;
            }
        }

        protected override void CheckTootipArea(Vector2 local)
        {
            if (dataPosList.Count <= 0) return;
            m_Tooltip.dataIndex[0] = -1;
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (!IsActive(i)) continue;
                for (int j = 0; j < dataPosList[i].Count; j++)
                {
                    if (Vector3.Distance(local, dataPosList[i][j]) <= m_Radar.linePointSize * 1.2f)
                    {
                        m_Tooltip.dataIndex[0] = i;
                        break;
                    }
                }
            }
            if (m_Tooltip.dataIndex[0] >= 0)
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                RefreshTooltip();
                if (m_Tooltip.lastDataIndex != m_Tooltip.dataIndex)
                {
                    RefreshChart();
                }
                m_Tooltip.lastDataIndex = m_Tooltip.dataIndex;
            }
            else
            {
                m_Tooltip.SetActive(false);
            }
        }

        protected override void RefreshTooltip()
        {
            base.RefreshTooltip();
            int index = m_Tooltip.dataIndex[0];
            if (index < 0)
            {
                m_Tooltip.SetActive(false);
                return;
            }
            m_Tooltip.SetActive(true);
            StringBuilder sb = new StringBuilder(m_Legend.data[index]);
            for (int i = 0; i < m_Radar.indicatorList.Count; i++)
            {
                string key = m_Radar.indicatorList[i].name;
                float value = m_Series.series[index].yData[i];
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
