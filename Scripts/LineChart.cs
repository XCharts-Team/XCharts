using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public class LineInfo
    {
        public float tickness = 0.8f;

        [Header("Point")]
        public bool showPoint = true;
        public float pointWid = 2.5f;

        [Header("Smooth")]
        public bool smooth = false;

        [Range(1f, 10f)]
        public float smoothStyle = 2f;

        [Header("Area")]
        public bool area = false;
        public Color areaColor;
    }

    public class LineChart : BaseAxesChart
    {
        [SerializeField]
        private LineInfo lineInfo;

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
            float scaleWid = coordinateWid / (xAxis.splitNumber - 1);
            for (int j = 0; j < seriesCount; j++)
            {
                if (!legend.IsShowSeries(j)) continue;
                Series series = seriesList[j];
                Color color = themeInfo.GetColor(j);
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;
                float startX = zeroX + (xAxis.boundaryGap ? scaleWid / 2 : 0);
                int showDataNumber = series.showDataNumber;
                int startIndex = 0;
                if (series.showDataNumber > 0 && series.dataList.Count > series.showDataNumber)
                {
                    startIndex = series.dataList.Count - series.showDataNumber;
                }
                for (int i = startIndex; i < series.dataList.Count; i++)
                {
                    SeriesData data = series.dataList[i];

                    np = new Vector3(startX + i * scaleWid, zeroY + data.value * coordinateHig / max);
                    if (i > 0)
                    {
                        if (lineInfo.smooth)
                        {
                            var list = ChartUtils.GetBezierList(lp, np, lineInfo.smoothStyle);
                            Vector3 start, to;
                            start = list[0];
                            for (int k = 1; k < list.Count; k++)
                            {
                                to = list[k];
                                ChartUtils.DrawLine(vh, start, to, lineInfo.tickness, color);
                                start = to;
                            }
                        }
                        else
                        {
                            ChartUtils.DrawLine(vh, lp, np, lineInfo.tickness, color);
                            if (lineInfo.area)
                            {
                                ChartUtils.DrawPolygon(vh, lp, np, new Vector3(np.x, zeroY),
                                    new Vector3(lp.x, zeroY), color);
                            }
                        }

                    }
                    lp = np;
                }
                // draw point
                if (lineInfo.showPoint)
                {
                    for (int i = 0; i < series.dataList.Count; i++)
                    {
                        SeriesData data = series.dataList[i];

                        Vector3 p = new Vector3(startX + i * scaleWid, 
                            zeroY + data.value * coordinateHig / max);
                        if(theme == Theme.Dark)
                        {
                            ChartUtils.DrawCricle(vh, p, lineInfo.pointWid, color, 
                                (int)lineInfo.pointWid * 5);
                        }
                        else
                        {
                            ChartUtils.DrawCricle(vh, p, lineInfo.pointWid, Color.white);
                            ChartUtils.DrawDoughnut(vh, p, lineInfo.pointWid - lineInfo.tickness,
                                lineInfo.pointWid, 0, 360, color);
                        }
                    }
                }
            }

        }
    }
}
