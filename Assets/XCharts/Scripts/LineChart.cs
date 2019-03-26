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

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            int seriesCount = seriesList.Count;
            float max = GetMaxValue();
            float scaleWid = coordinateWid / (xAxis.splitNumber - 1);
            for (int j = 0; j < seriesCount; j++)
            {
                if (!legend.IsShowSeries(j)) continue;
                Series series = seriesList[j];
                Color32 color = themeInfo.GetColor(j);
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
                    float value = series.dataList[i];

                    np = new Vector3(startX + i * scaleWid, zeroY + value * coordinateHig / max);
                    if (i > 0)
                    {
                        if (lineInfo.smooth)
                        {
                            var list = ChartUtils.GetBezierList(lp, np, lineInfo.smoothStyle);
                            Vector3 start, to;
                            start = list[0];
                            for (int k = 1; k < list.Length; k++)
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
                        float value = series.dataList[i];

                        Vector3 p = new Vector3(startX + i * scaleWid,
                            zeroY + value * coordinateHig / max);
                        float pointWid = lineInfo.pointWid;
                        if (tooltip.show && i == tooltip.DataIndex - 1)
                        {
                            pointWid = pointWid * 1.8f;
                        }
                        if (theme == Theme.Dark)
                        {

                            ChartUtils.DrawCricle(vh, p, pointWid, color,
                                (int)lineInfo.pointWid * 5);
                        }
                        else
                        {
                            ChartUtils.DrawCricle(vh, p, pointWid, Color.white);
                            ChartUtils.DrawDoughnut(vh, p, pointWid - lineInfo.tickness,
                                pointWid, 0, 360, color);
                        }
                    }
                }
            }
            //draw tooltip line
            if (tooltip.show && tooltip.DataIndex > 0)
            {
                float splitWid = coordinateWid / (xAxis.splitNumber - 1);
                float px = zeroX + (tooltip.DataIndex - 1) * splitWid + (xAxis.boundaryGap ? splitWid / 2 : 0);
                Vector2 sp = new Vector2(px, zeroY);
                Vector2 ep = new Vector2(px, zeroY + coordinateHig);
                ChartUtils.DrawLine(vh, sp, ep, coordinate.tickness, themeInfo.tooltipFlagAreaColor);
            }
        }
    }
}
