
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public class LineData
    {
        public bool smooth = false;
        public bool area = false;
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
                        if (lineData.smooth)
                        {
                            var list = ChartUtils.GetBezierList(lp, np);
                            Vector3 start, to;
                            start = list[0];
                            for(int k = 1; k < list.Count; k++)
                            {
                                to = list[k];
                                ChartUtils.DrawLine(vh, start, to, lineData.tickness, color);
                                start = to;
                            }
                        }
                        else
                        {
                            ChartUtils.DrawLine(vh, lp, np, lineData.tickness, color);
                            if (lineData.area)
                            {
                                ChartUtils.DrawPolygon(vh, lp, np, new Vector3(np.x, zeroY), new Vector3(lp.x, zeroY), color);
                            }
                        }
                        
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
