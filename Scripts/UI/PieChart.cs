using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/PieChart", 15)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class PieChart : BaseChart
    {
        [SerializeField] private Pie m_Pie = Pie.defaultPie;

        private float m_PieCenterX = 0f;
        private float m_PieCenterY = 0f;
        private float m_PieRadius = 0;
        private Vector2 m_PieCenter;
        private List<float> m_AngleList = new List<float>();

        public Pie pie { get { return m_Pie; } }

        /// <summary>
        /// Add a data to pie.
        /// </summary>
        /// <param name="serieName">the name of data</param>
        /// <param name="value">the data</param>
        /// <returns>Return true forever</returns>
        public override bool AddData(string serieName, float value)
        {
            m_Legend.AddData(serieName);
            var serie = m_Series.AddSerie(serieName, SerieType.Pie);
            serie.ClearData();
            serie.AddYData(value);
            RefreshChart();
            return true;
        }

        /// <summary>
        /// Update the data for the specified name.
        /// </summary>
        /// <param name="legend">the name of data</param>
        /// <param name="value">the data</param>
        /// <param name="dataIndex">is not used in this function</param>
        public override void UpdateData(string legend, float value, int dataIndex = 0)
        {
            var serie = m_Series.GetSerie(legend);
            if (serie != null)
            {
                serie.UpdateYData(0, value);
                RefreshChart();
            }
        }

        protected override void Awake()
        {
            raycastTarget = m_Pie.selected;
        }

        protected override void Update()
        {
            base.Update();
            if (raycastTarget != m_Pie.selected)
            {
                raycastTarget = m_Pie.selected;
                RefreshChart();
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Pie = Pie.defaultPie;
            m_Title.text = "PieChart";
            RemoveData();
            AddData("serie1", 80);
            AddData("serie2", 20);
        }
#endif

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            UpdatePieCenter();
            float totalDegree = 360;
            float startDegree = 0;
            float dataTotal = GetDataTotal();
            float dataMax = GetDataMax();
            m_AngleList.Clear();
            HashSet<string> serieNameSet = new HashSet<string>();
            int serieNameCount = -1;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.series[i];
                serie.index = i;
                var data = serie.yData;
                if (string.IsNullOrEmpty(serie.name)) serieNameCount++;
                else if (!serieNameSet.Contains(serie.name))
                {
                    serieNameSet.Add(serie.name);
                    serieNameCount++;
                }
                if (data.Count <= 0 || !serie.show)
                {
                    m_AngleList.Add(i > 0 ? m_AngleList[i - 1] : 0);
                    continue;
                }
                float value = data[0];
                float degree = totalDegree * value / dataTotal;
                float toDegree = startDegree + degree;

                float outSideRadius = m_Pie.rose ?
                    m_Pie.insideRadius + (m_PieRadius - m_Pie.insideRadius) * value / dataMax :
                    m_PieRadius;
                if (m_Tooltip.show && m_Tooltip.dataIndex[0] == i)
                {
                    outSideRadius += m_Pie.tooltipExtraRadius;
                }
                var offset = m_Pie.space;
                if (m_Pie.selected && m_Pie.selectedIndex == i)
                {
                    offset += m_Pie.selectedOffset;
                }
                if (offset > 0)
                {
                    float currAngle = (startDegree + (toDegree - startDegree) / 2) * Mathf.Deg2Rad;
                    var offestCenter = new Vector3(m_PieCenter.x + offset * Mathf.Sin(currAngle),
                        m_PieCenter.y + offset * Mathf.Cos(currAngle));
                    ChartHelper.DrawDoughnut(vh, offestCenter, m_Pie.insideRadius, outSideRadius,
                        startDegree, toDegree, m_ThemeInfo.GetColor(serieNameCount));
                }
                else
                {
                    ChartHelper.DrawDoughnut(vh, m_PieCenter, m_Pie.insideRadius, outSideRadius,
                        startDegree, toDegree, m_ThemeInfo.GetColor(serieNameCount));
                }
                m_AngleList.Add(toDegree);
                startDegree = toDegree;
            }
        }

        protected override void OnLegendButtonClicked()
        {
            base.OnLegendButtonClicked();

        }

        private float GetDataTotal()
        {
            float total = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (IsActive(i))
                {
                    total += m_Series.series[i].GetYData(0);
                }
            }
            return total;
        }

        private float GetDataMax()
        {
            float max = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (IsActive(i) && m_Series.series[i].GetYData(0) > max)
                {
                    max = m_Series.series[i].GetYData(0);
                }
            }
            return max;
        }

        private void UpdatePieCenter()
        {
            float diffX = chartWidth - m_Pie.left - m_Pie.right;
            float diffY = chartHeight - m_Pie.top - m_Pie.bottom;
            float diff = Mathf.Min(diffX, diffY);
            if (m_Pie.outsideRadius <= 0)
            {
                m_PieRadius = diff / 3 * 2;
                m_PieCenterX = m_Pie.left + m_PieRadius;
                m_PieCenterY = m_Pie.bottom + m_PieRadius;
            }
            else
            {
                m_PieRadius = m_Pie.outsideRadius;
                m_PieCenterX = chartWidth / 2;
                m_PieCenterY = chartHeight / 2;
                if (m_Pie.left > 0) m_PieCenterX = m_Pie.left + m_PieRadius;
                if (m_Pie.right > 0) m_PieCenterX = chartWidth - m_Pie.right - m_PieRadius;
                if (m_Pie.top > 0) m_PieCenterY = chartHeight - m_Pie.top - m_PieRadius;
                if (m_Pie.bottom > 0) m_PieCenterY = m_Pie.bottom + m_PieRadius;
            }
            m_PieCenter = new Vector2(m_PieCenterX, m_PieCenterY);
        }

        protected override void CheckTootipArea(Vector2 local)
        {
            float dist = Vector2.Distance(local, m_PieCenter);
            if (dist > m_PieRadius)
            {
                m_Tooltip.dataIndex[0] = -1;
                m_Tooltip.SetActive(false);
            }
            else
            {
                m_Tooltip.dataIndex[0] = GetPosPieIndex(local);
            }
            if (m_Tooltip.dataIndex[0] >= 0)
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                RefreshTooltip();
                if (m_Tooltip.IsSelected())
                {
                    m_Tooltip.UpdateLastDataIndex();
                    RefreshChart();
                }
            }
        }

        private int GetPosPieIndex(Vector2 local)
        {
            Vector2 dir = local - m_PieCenter;
            float angle = VectorAngle(Vector2.up, dir);
            for (int i = m_AngleList.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    if (angle <= m_AngleList[i]) return m_Tooltip.dataIndex[0] = 0;
                }
                else if (angle <= m_AngleList[i] && angle > m_AngleList[i - 1])
                {
                    return m_Tooltip.dataIndex[0] = i;
                }
            }
            return -1;
        }

        float VectorAngle(Vector2 from, Vector2 to)
        {
            float angle;

            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            angle = cross.z > 0 ? -angle : angle;
            angle = (angle + 360) % 360;
            return angle;
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
            string strColor = ColorUtility.ToHtmlStringRGBA(m_ThemeInfo.GetColor(index));
            string key = m_Series.series[index].name;
            if (string.IsNullOrEmpty(key)) key = m_Legend.GetData(index);
            float value = m_Series.series[index].yData[0];
            string txt = "";
            if (!string.IsNullOrEmpty(m_Pie.name))
            {
                txt += m_Pie.name + "\n";
            }
            txt += string.Format("<color=#{0}>● </color>{1}: {2}", strColor, key, value);
            m_Tooltip.UpdateContentText(txt);

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

        public override void OnPointerDown(PointerEventData eventData)
        {
            Vector2 local;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                eventData.position, canvas.worldCamera, out local))
            {
                return;
            }
            var selectedIndex = GetPosPieIndex(local);
            if (selectedIndex != m_Pie.selectedIndex)
            {
                m_Pie.selectedIndex = selectedIndex;
            }
            else
            {
                m_Pie.selectedIndex = -1;
            }
            RefreshChart();
        }
    }
}
