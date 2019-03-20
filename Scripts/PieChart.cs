using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public class PieInfo
    {
        public string name;
        public float insideRadius = 0f;
        public float outsideRadius = 80f;
        public float tooltipExtraRadius = 10f;
        public bool outsideRadiusDynamic = false;
        public float space;
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    public class PieChart : BaseChart
    {
        [SerializeField]
        private PieInfo pieInfo = new PieInfo();

        private float pieCenterX = 0f;
        private float pieCenterY = 0f;
        private float pieRadius = 0;
        private Vector2 pieCenter;
        private List<float> angleList = new List<float>();

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
            angleList.Clear();
            for (int i = 0; i < seriesList.Count; i++)
            {
                if (!legend.IsShowSeries(i))
                {
                    angleList.Add(0);
                    continue;
                }
                float value = seriesList[i].dataList[0];
                float degree = totalDegree * value / dataTotal;
                float toDegree = startDegree + degree;

                float outSideRadius = pieInfo.outsideRadiusDynamic ?
                    pieInfo.insideRadius + (pieRadius - pieInfo.insideRadius) * value / dataMax :
                    pieRadius;
                if (tooltip.show && tooltip.DataIndex == i + 1)
                {
                    outSideRadius += pieInfo.tooltipExtraRadius;
                }
                ChartUtils.DrawDoughnut(vh, pieCenter, pieInfo.insideRadius,
                    outSideRadius, startDegree, toDegree, themeInfo.GetColor(i));
                angleList.Add(toDegree);
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
            for (int i = 0; i < seriesList.Count; i++)
            {
                if (legend.IsShowSeries(i))
                {
                    total += seriesList[i].GetData(0);
                }
            }
            return total;
        }

        private float GetDataMax()
        {
            float max = 0;
            for (int i = 0; i < seriesList.Count; i++)
            {
                if (legend.IsShowSeries(i) && seriesList[i].GetData(0) > max)
                {
                    max = seriesList[i].GetData(0);
                }
            }
            return max;
        }

        private void UpdatePieCenter()
        {
            float diffX = chartWid - pieInfo.left - pieInfo.right;
            float diffY = chartHig - pieInfo.top - pieInfo.bottom;
            float diff = Mathf.Min(diffX, diffY);
            if (pieInfo.outsideRadius <= 0)
            {
                pieRadius = diff / 3 * 2;
                pieCenterX = pieInfo.left + pieRadius;
                pieCenterY = pieInfo.bottom + pieRadius;
            }
            else
            {
                pieRadius = pieInfo.outsideRadius;
                pieCenterX = chartWid / 2;
                pieCenterY = chartHig / 2;
                if (pieInfo.left > 0) pieCenterX = pieInfo.left + pieRadius;
                if (pieInfo.right > 0) pieCenterX = chartWid - pieInfo.right - pieRadius;
                if (pieInfo.top > 0) pieCenterY = chartHig - pieInfo.top - pieRadius;
                if (pieInfo.bottom > 0) pieCenterY = pieInfo.bottom + pieRadius;
            }
            pieCenter = new Vector2(pieCenterX, pieCenterY);
        }

        protected override void CheckTootipArea(Vector2 local)
        {

            float dist = Vector2.Distance(local, pieCenter);
            if (dist > pieRadius)
            {
                tooltip.DataIndex = 0;
                tooltip.SetActive(false);
            }
            else
            {
                Vector2 dir = local - pieCenter;
                float angle = VectorAngle(Vector2.up, dir);
                tooltip.DataIndex = 0;
                for (int i = angleList.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && angle < angleList[i])
                    {
                        tooltip.DataIndex = 1;
                        break;
                    }
                    else if (angle < angleList[i] && angle > angleList[i - 1])
                    {
                        tooltip.DataIndex = i + 1;
                        break;
                    }
                }
            }
            if (tooltip.DataIndex > 0)
            {
                tooltip.UpdatePos(new Vector2(local.x + 18, local.y - 25));
                RefreshTooltip();
                if (tooltip.LastDataIndex != tooltip.DataIndex)
                {
                    RefreshChart();
                }
                tooltip.LastDataIndex = tooltip.DataIndex;
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
            int index = tooltip.DataIndex - 1;
            if (index < 0)
            {
                tooltip.SetActive(false);
                return;
            }
            tooltip.SetActive(true);
            string strColor = ColorUtility.ToHtmlStringRGBA(themeInfo.GetColor(index));
            string key = legend.dataList[index];
            float value = seriesList[index].dataList[0];
            string txt = "";
            if (!string.IsNullOrEmpty(pieInfo.name))
            {
                txt += pieInfo.name + "\n";
            }
            txt += string.Format("<color=#{0}>● </color>{1}: {2}", strColor, key, value);
            tooltip.UpdateTooltipText(txt);

            var pos = tooltip.GetPos();
            if (pos.x + tooltip.Width > chartWid)
            {
                pos.x = chartWid - tooltip.Width;
            }
            if (pos.y - tooltip.Height < 0)
            {
                pos.y = tooltip.Height;
            }
            tooltip.UpdatePos(pos);
        }
    }
}
