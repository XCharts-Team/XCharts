using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public enum PointType
    {
        square,
        cicle
    }

    [System.Serializable]
    public class LineData
    {

        public float tickness = 0.8f;

        [Header("Point")]
        public bool showPoint = true;
        public PointType pointType = PointType.square;
        public float pointWid = 1.0f;
        public Color pointColor = Color.white;

        [Header("Smooth")]
        public bool smooth = false;

        [Range(1f, 10f)]
        public float smoothStyle = 2f;

        [Header("Area")]
        public bool area = false;
        public Color areaStartColor;
        public Color areaToColor;
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
            float scaleWid = coordinateWid / (xAxis.splitNumber - 1);
            for (int j = 0; j < seriesCount; j++)
            {
                if (!legend.IsShowSeries(j)) continue;
                Series series = seriesList[j];
                Color color = legend.GetColor(j);
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
                        if (lineData.smooth)
                        {
                            var list = ChartUtils.GetBezierList(lp, np, lineData.smoothStyle);
                            Vector3 start, to;
                            start = list[0];
                            for (int k = 1; k < list.Count; k++)
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
                                ChartUtils.DrawPolygon(vh, lp, np, new Vector3(np.x, zeroY), new Vector3(lp.x, zeroY),
                                    lineData.areaStartColor, lineData.areaToColor);
                            }
                        }

                    }
                    lp = np;
                }
                // draw point
                if (lineData.showPoint)
                {
                    for (int i = 0; i < series.dataList.Count; i++)
                    {
                        SeriesData data = series.dataList[i];

                        Vector3 p = new Vector3(startX + i * scaleWid, zeroY + data.value * coordinateHig / max);
                        switch (lineData.pointType)
                        {
                            case PointType.square:
                                ChartUtils.DrawPolygon(vh, p, lineData.pointWid, lineData.pointColor);
                                break;
                            case PointType.cicle:
                                ChartUtils.DrawCricle(vh, p, lineData.pointWid, lineData.pointColor, (int)lineData.pointWid * 5);
                                break;
                        }
                    }
                }
            }

        }
    }
}
