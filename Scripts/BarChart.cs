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

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);

            if(yAxis.type == AxisType.category)
            {
                int seriesCount = seriesList.Count;
                float scaleWid = coordinateHig / (yAxis.splitNumber - 1);
                float barWid = barInfo.barWid > 1 ? barInfo.barWid : scaleWid * barInfo.barWid;
                float offset = (scaleWid - barWid * seriesCount - barInfo.space * (seriesCount - 1)) / 2;
                float max = GetMaxValue();
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
                        SeriesData data = series.dataList[i];
                        float pX = zeroX + coordinate.tickness;
                        float pY = zeroY + i * coordinateHig / (yAxis.splitNumber - 1);
                        if (!yAxis.boundaryGap) pY -= scaleWid / 2;
                        float barHig = data.value / max * coordinateWid;
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
                float scaleWid = coordinateWid / (xAxis.splitNumber - 1);
                float barWid = barInfo.barWid > 1 ? barInfo.barWid : scaleWid * barInfo.barWid;
                float offset = (scaleWid - barWid * seriesCount - barInfo.space * (seriesCount - 1)) / 2;
                float max = GetMaxValue();
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
                        SeriesData data = series.dataList[i];
                        float pX = zeroX + i * coordinateWid / (xAxis.splitNumber - 1);
                        if (!xAxis.boundaryGap) pX -= scaleWid / 2;
                        float pY = zeroY + coordinate.tickness;
                        float barHig = data.value / max * coordinateHig;
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
