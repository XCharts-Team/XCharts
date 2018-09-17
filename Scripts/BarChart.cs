using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{

    public class BarChart : BaseChart
    {
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
            foreach(var series in seriesList)
            {
                float scaleWid = coordinateWid / (xAxis.scaleNum - 1);
                float barWid = scaleWid * 0.6f;
                float space = (scaleWid - barWid) / 2;
                float max = series.max;
                for (int i = 0; i < series.dataList.Count; i++)
                {
                    SeriesData data = series.dataList[i];
                    float pX = zeroX + i * coordinateWid / (xAxis.scaleNum - 1);
                    float pY = zeroY + coordinate.tickness;
                    float barHig = data.value / max * coordinateHig;
                    Vector3 p1 = new Vector3(pX+space,pY);
                    Vector3 p2 = new Vector3(pX + space, pY + barHig);
                    Vector3 p3 = new Vector3(pX + space + barWid, pY + barHig);
                    Vector3 p4 = new Vector3(pX + space +barWid, pY);
                    ChartUtils.DrawPolygon(vh,p1,p2,p3,p4,Color.blue);
                }
            }
        }
    }
}
