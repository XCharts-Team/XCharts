using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public class PieChart : BaseChart
    {
        [SerializeField] private Pie m_Pie = Pie.defaultPie;

        private float m_PieCenterX = 0f;
        private float m_PieCenterY = 0f;
        private float m_PieRadius = 0;
        private Vector2 m_PieCenter;
        private List<float> m_AngleList = new List<float>();

        public Pie pie { get { return m_Pie; } }

        public override void AddData(string legend, float value)
        {
            m_Legend.AddData(legend);
            var serie = m_Series.AddData(legend,value);
            if (serie != null)
            {
                serie.ClearData();
                serie.AddData(value);
                RefreshChart();
            }
        }

        public override void UpdateData(string legend, float value, int dataIndex = 0)
        {
            var serie = m_Series.GetSerie(legend);
            if (serie != null)
            {
                serie.UpdateData(0, value);
                RefreshChart();
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            UpdatePieCenter();
            float totalDegree = 360;
            float startDegree = 0;
            float dataTotal = GetDataTotal();
            float dataMax = GetDataMax();
            m_AngleList.Clear();
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (!IsActive(i))
                {
                    m_AngleList.Add(0);
                    continue;
                }
                float value = m_Series.series[i].data[0];
                float degree = totalDegree * value / dataTotal;
                float toDegree = startDegree + degree;

                float outSideRadius = m_Pie.rose ?
                    m_Pie.insideRadius + (m_PieRadius - m_Pie.insideRadius) * value / dataMax :
                    m_PieRadius;
                if (m_Tooltip.show && m_Tooltip.dataIndex == i + 1)
                {
                    outSideRadius += m_Pie.tooltipExtraRadius;
                }
                ChartHelper.DrawDoughnut(vh, m_PieCenter, m_Pie.insideRadius,
                    outSideRadius, startDegree, toDegree, m_ThemeInfo.GetColor(i));
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
                    total += m_Series.series[i].GetData(0);
                }
            }
            return total;
        }

        private float GetDataMax()
        {
            float max = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (IsActive(i) && m_Series.series[i].GetData(0) > max)
                {
                    max = m_Series.series[i].GetData(0);
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
                m_Tooltip.dataIndex = 0;
                m_Tooltip.SetActive(false);
            }
            else
            {
                Vector2 dir = local - m_PieCenter;
                float angle = VectorAngle(Vector2.up, dir);
                m_Tooltip.dataIndex = 0;
                for (int i = m_AngleList.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && angle < m_AngleList[i])
                    {
                        m_Tooltip.dataIndex = 1;
                        break;
                    }
                    else if (angle < m_AngleList[i] && angle > m_AngleList[i - 1])
                    {
                        m_Tooltip.dataIndex = i + 1;
                        break;
                    }
                }
            }
            if (m_Tooltip.dataIndex > 0)
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                RefreshTooltip();
                if (m_Tooltip.lastDataIndex != m_Tooltip.dataIndex)
                {
                    RefreshChart();
                }
                m_Tooltip.lastDataIndex = m_Tooltip.dataIndex;
            }
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
            int index = m_Tooltip.dataIndex - 1;
            if (index < 0)
            {
                m_Tooltip.SetActive(false);
                return;
            }
            m_Tooltip.SetActive(true);
            string strColor = ColorUtility.ToHtmlStringRGBA(m_ThemeInfo.GetColor(index));
            string key = m_Legend.data[index];
            float value = m_Series.series[index].data[0];
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
    }
}
