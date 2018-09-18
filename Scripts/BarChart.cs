using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public class BarData
    {
        public float barWid = 0.7f;
        public float space;
    }

    public class BarChart : BaseChart
    {
        [SerializeField]
        private BarData barData;

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
                float seriesCount = seriesList.Count;
                float scaleWid = coordinateHig / (yAxis.scaleNum - 1);
                float barWid = barData.barWid > 1 ? barData.barWid : scaleWid * barData.barWid;
                float offset = (scaleWid - barWid * seriesCount - barData.space * (seriesCount - 1)) / 2;
                float max = GetMaxValue();
                for (int j = 0; j < seriesCount; j++)
                {
                    if (!legend.IsShowSeries(j)) continue;
                    Series series = seriesList[j];
                    Color color = legend.GetColor(j);
                    for (int i = 0; i < series.dataList.Count; i++)
                    {
                        SeriesData data = series.dataList[i];
                        float pX = zeroX + coordinate.tickness;
                        float pY = zeroY + i * coordinateHig / (yAxis.scaleNum - 1);
                        float barHig = data.value / max * coordinateWid;
                        float space = offset + j * (barWid + barData.space);
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
                float seriesCount = seriesList.Count;
                float scaleWid = coordinateWid / (xAxis.scaleNum - 1);
                float barWid = barData.barWid > 1 ? barData.barWid : scaleWid * barData.barWid;
                float offset = (scaleWid - barWid * seriesCount - barData.space * (seriesCount - 1)) / 2;
                float max = GetMaxValue();
                for (int j = 0; j < seriesCount; j++)
                {
                    if (!legend.IsShowSeries(j)) continue;
                    Series series = seriesList[j];
                    Color color = legend.GetColor(j);
                    for (int i = 0; i < series.dataList.Count; i++)
                    {
                        SeriesData data = series.dataList[i];
                        float pX = zeroX + i * coordinateWid / (xAxis.scaleNum - 1);
                        float pY = zeroY + coordinate.tickness;
                        float barHig = data.value / max * coordinateHig;
                        float space = offset + j * (barWid + barData.space);
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
