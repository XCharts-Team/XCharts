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
        public float radius = 80f;
        public float space;
        public float left;
        public float right;
        public float top;
        public float bottom;
        public List<PieData> dataList;

        public float GetDataTotal()
        {
            float total = 0;
            foreach(var d in dataList)
            {
                total += d.value;
            }
            return total;
        }
    }

    public class PieChart : BaseChart
    {
        [SerializeField]
        private PieInfo pieInfo;

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
            for (int i = 0; i < pieInfo.dataList.Count; i++)
            {
                if (!legend.IsShowSeries(i)) continue;
                float value = pieInfo.dataList[i].value;
                float degree = totalDegree * value / dataTotal;
                float toDegree = startDegree + degree;
                ChartUtils.DrawSector(vh, new Vector3(pieCenterX, pieCenterY), pieRadius, legend.GetColor(i), 360, 
                    startDegree, toDegree);
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

        private void UpdatePieCenter()
        {
            float diffX = chartWid - pieInfo.left - pieInfo.right;
            float diffY = chartHig - pieInfo.top - pieInfo.bottom;
            float diff = Mathf.Min(diffX, diffY);
            if(pieInfo.radius <= 0)
            {
                pieRadius = diff / 3 * 2;
                pieCenterX = pieInfo.left + pieRadius;
                pieCenterY = pieInfo.bottom + pieRadius;
            }
            else
            {
                pieRadius = pieInfo.radius;
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
