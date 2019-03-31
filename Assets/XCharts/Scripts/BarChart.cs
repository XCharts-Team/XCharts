using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public class BarInfo
    {
        public float barWid = 0.7f;
        public float space;
    }

    public class BarChart : BaseAxesChart
    {
        [SerializeField]
        private BarInfo barInfo = new BarInfo();

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
            if (yAxis.type == AxisType.category)
            {
                int seriesCount = seriesList.Count;
                float scaleWid = yAxis.GetSplitWidth(coordinateHig);
                float barWid = barInfo.barWid > 1 ? barInfo.barWid : scaleWid * barInfo.barWid;
                float offset = (scaleWid - barWid * seriesCount - barInfo.space * (seriesCount - 1)) / 2;
                float max = GetMaxValue();
                if (tooltip.show && tooltip.DataIndex > 0)
                {
                    float pX = zeroX + coordinateWid;
                    float pY = zeroY + scaleWid * (tooltip.DataIndex - 1);
                    Vector3 p1 = new Vector3(zeroX, pY);
                    Vector3 p2 = new Vector3(zeroX, pY + scaleWid);
                    Vector3 p3 = new Vector3(pX, pY + scaleWid);
                    Vector3 p4 = new Vector3(pX, pY);
                    ChartUtils.DrawPolygon(vh, p1, p2, p3, p4, themeInfo.tooltipFlagAreaColor);
                }
                for (int j = 0; j < seriesCount; j++)
                {
                    if (!legend.IsShowSeries(j)) continue;
                    Series series = seriesList[j];
                    Color color = themeInfo.GetColor(j);
                    int startIndex = 0;
                    if (series.showDataNumber > 0 && series.dataList.Count > series.showDataNumber)
                    {
                        startIndex = series.dataList.Count - series.showDataNumber;
                    }
                    for (int i = startIndex; i < series.dataList.Count; i++)
                    {
                        float data = series.dataList[i];
                        float pX = zeroX + coordinate.tickness;
                        float pY = zeroY + i * scaleWid;
                        if (!yAxis.boundaryGap) pY -= scaleWid / 2;
                        float barHig = data / max * coordinateWid;
                        float space = offset + j * (barWid + barInfo.space);
                        Vector3 p1 = new Vector3(pX, pY + space + barWid);
                        Vector3 p2 = new Vector3(pX + barHig, pY + space + barWid);
                        Vector3 p3 = new Vector3(pX + barHig, pY + space);
                        Vector3 p4 = new Vector3(pX, pY + space);
                        ChartUtils.DrawPolygon(vh, p1, p2, p3, p4, color);
                    }
                }
            }
            else
            {
                int seriesCount = seriesList.Count;
                float scaleWid = xAxis.GetDataWidth(coordinateWid);
                float barWid = barInfo.barWid > 1 ? barInfo.barWid : scaleWid * barInfo.barWid;
                float offset = (scaleWid - barWid * seriesCount - barInfo.space * (seriesCount - 1)) / 2;
                float max = GetMaxValue();
                if (tooltip.show && tooltip.DataIndex > 0)
                {
                    float pX = zeroX + scaleWid * (tooltip.DataIndex - 1);
                    float pY = zeroY + coordinateHig;
                    Vector3 p1 = new Vector3(pX, zeroY);
                    Vector3 p2 = new Vector3(pX, pY);
                    Vector3 p3 = new Vector3(pX + scaleWid, pY);
                    Vector3 p4 = new Vector3(pX + scaleWid, zeroY);
                    ChartUtils.DrawPolygon(vh, p1, p2, p3, p4, themeInfo.tooltipFlagAreaColor);
                }
                for (int j = 0; j < seriesCount; j++)
                {
                    if (!legend.IsShowSeries(j)) continue;
                    Series series = seriesList[j];
                    Color color = themeInfo.GetColor(j);
                    int startIndex = 0;
                    if (series.showDataNumber > 0 && series.dataList.Count > series.showDataNumber)
                    {
                        startIndex = series.dataList.Count - series.showDataNumber;
                    }
                    for (int i = startIndex; i < series.dataList.Count; i++)
                    {
                        float data = series.dataList[i];
                        float pX = zeroX + i * scaleWid;
                        if (!xAxis.boundaryGap) pX -= scaleWid / 2;
                        float pY = zeroY + coordinate.tickness;
                        float barHig = data / max * coordinateHig;
                        float space = offset + j * (barWid + barInfo.space);
                        Vector3 p1 = new Vector3(pX + space, pY);
                        Vector3 p2 = new Vector3(pX + space, pY + barHig);
                        Vector3 p3 = new Vector3(pX + space + barWid, pY + barHig);
                        Vector3 p4 = new Vector3(pX + space + barWid, pY);
                        ChartUtils.DrawPolygon(vh, p1, p2, p3, p4, color);
                    }
                }
            }
        }
    }
}
