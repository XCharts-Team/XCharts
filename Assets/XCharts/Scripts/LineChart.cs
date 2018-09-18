
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public class LineData
    {
        public float pointWid = 1;
        public float tickness = 0.8f;
    }

    public class LineChart : BaseChart
    {
        [SerializeField]
        private LineData lineData;

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
            int seriesCount = seriesList.Count;
            float max = GetMaxValue();
            float scaleWid = coordinateWid / (xAxis.scaleNum - 1);
            for (int j = 0; j < seriesCount; j++)
            {
                if (!legend.IsShowSeries(j)) continue;
                Series series = seriesList[j];
                Color color = legend.GetColor(j);
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;
                float startX = zeroX + scaleWid / 2;
                for (int i = 0; i < series.dataList.Count; i++)
                {
                    SeriesData data = series.dataList[i];

                    np = new Vector3(startX + i * scaleWid, zeroY + data.value * coordinateHig / max);
                    if (i > 0)
                    {
                        ChartUtils.DrawLine(vh, lp, np, lineData.tickness, color);
                    }
                    lp = np;
                }
                // draw point
                for (int i = 0; i < series.dataList.Count; i++)
                {
                    SeriesData data = series.dataList[i];

                    Vector3 p = new Vector3(startX + i * scaleWid, zeroY + data.value * coordinateHig / max);
                    ChartUtils.DrawPolygon(vh, p, lineData.pointWid, Color.white);
                }
            }

        }
    }
}
