using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public class PieData
    {
        public string text;
        public float value;
    }

    [System.Serializable]
    public class PieInfo
    {
        public float insideRadius = 0f;
        public float outsideRadius = 80f;
        public bool outsideRadiusDynamic = false;
        public float space;
        public float left;
        public float right;
        public float top;
        public float bottom;
        public List<PieData> dataList = new List<PieData>();
    }

    public class PieChart : BaseChart
    {
        [SerializeField]
        private PieInfo pieInfo = new PieInfo();

        private float pieCenterX = 0f;
        private float pieCenterY = 0f;
        private float pieRadius = 0;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);
            UpdatePieCenter();
            float totalDegree = 360;
            float startDegree = 0;
            float dataTotal = GetDataTotal();
            float dataMax = GetDataMax();
            for (int i = 0; i < pieInfo.dataList.Count; i++)
            {
                if (!legend.IsShowSeries(i)) continue;
                float value = pieInfo.dataList[i].value;
                float degree = totalDegree * value / dataTotal;
                float toDegree = startDegree + degree;

                float outSideRadius = pieInfo.outsideRadiusDynamic ? 
                    pieInfo.insideRadius + (pieRadius - pieInfo.insideRadius) * value / dataMax : 
                    pieRadius;
                ChartUtils.DrawDoughnut(vh, new Vector3(pieCenterX, pieCenterY), pieInfo.insideRadius, 
                    outSideRadius,startDegree, toDegree, themeInfo.GetColor(i));
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
            for(int i = 0; i < pieInfo.dataList.Count; i++)
            {
                if (legend.IsShowSeries(i))
                {
                    total += pieInfo.dataList[i].value;
                }
            }
            return total;
        }

        private float GetDataMax()
        {
            float max = 0;
            for(int i = 0; i < pieInfo.dataList.Count; i++)
            {
                if(legend.IsShowSeries(i) && pieInfo.dataList[i].value > max)
                {
                    max = pieInfo.dataList[i].value;
                }
            }
            return max;
        }

        private void UpdatePieCenter()
        {
            float diffX = chartWid - pieInfo.left - pieInfo.right;
            float diffY = chartHig - pieInfo.top - pieInfo.bottom;
            float diff = Mathf.Min(diffX, diffY);
            if(pieInfo.outsideRadius <= 0)
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
        }
    }
}
