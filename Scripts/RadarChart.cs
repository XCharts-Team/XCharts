using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public class RadarIndicator
    {
        public string name;
        public float max;
    }

    [System.Serializable]
    public class RadarInfo
    {
        public float radius = 100;
        public int splitNumber = 5;

        public float space;
        public float left;
        public float right;
        public float top;
        public float bottom;

        public float lineTickness = 1f;
        public float linePointSize = 5f;
        public Color lineColor = Color.grey;
        public List<Color> backgroundColorList;
        public List<RadarIndicator> indicatorList;
    }

    public class RadarChart : BaseChart
    {
        [SerializeField]
        private RadarInfo radarInfo;

        private float radarCenterX = 0f;
        private float radarCenterY = 0f;
        private float radarRadius = 0;

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
            UpdateRadarCenter();
            DrawRadar(vh);
            DrawData(vh);
        }

        protected override void OnLegendButtonClicked()
        {
            base.OnLegendButtonClicked();
        }

        private void DrawData(VertexHelper vh)
        {
            int indicatorNum = radarInfo.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum;
            var p = new Vector3(radarCenterX, radarCenterY);
            Vector3 startPoint = Vector3.zero;
            Vector3 toPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;
            
            for (int i = 0; i < seriesList.Count; i++)
            {
                if (!legend.IsShowSeries(i)) continue;
                var dataList = seriesList[i].dataList;
                var color = legend.GetColor(i);
                var max = radarInfo.indicatorList[i].max > 0 ?
                    radarInfo.indicatorList[i].max :
                    GetMaxValue();
                List<Vector3> pointList = new List<Vector3>();
                for (int j = 0; j < dataList.Count; j++)
                {
                    var radius = radarInfo.radius * dataList[j].value / max;
                    var currAngle = j * angle;
                    if (j == 0)
                    {
                        startPoint = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                            p.y + radius * Mathf.Cos(currAngle));
                        firstPoint = startPoint;
                    }
                    else
                    {
                        toPoint = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                            p.y + radius * Mathf.Cos(currAngle));
                        ChartUtils.DrawLine(vh, startPoint, toPoint, radarInfo.lineTickness, color);
                        startPoint = toPoint;
                    }
                    pointList.Add(startPoint);
                }
                ChartUtils.DrawLine(vh, startPoint, firstPoint, radarInfo.lineTickness, color);
                foreach (var point in pointList)
                {
                    float radius = radarInfo.linePointSize - radarInfo.lineTickness * 2;

                    ChartUtils.DrawCricle(vh, point, radius, Color.white);
                    ChartUtils.DrawDoughnut(vh, point, radius, radarInfo.linePointSize, 0, 360, color);
                }
            }
        }

        private void DrawRadar(VertexHelper vh)
        {
            float insideRadius = 0, outsideRadius = 0;
            float block = radarInfo.radius / radarInfo.splitNumber;
            int indicatorNum = radarInfo.indicatorList.Count;
            Vector3 p1, p2, p3, p4;
            Vector3 p = new Vector3(radarCenterX,radarCenterY);
            float angle = 2 * Mathf.PI / indicatorNum;
            for (int i = 0; i < radarInfo.splitNumber; i++)
            {
                Color color = radarInfo.backgroundColorList[i% radarInfo.backgroundColorList.Count];
                outsideRadius = insideRadius + block;
                p1 = new Vector3(p.x + insideRadius * Mathf.Sin(0), p.y + insideRadius * Mathf.Cos(0));
                p2 = new Vector3(p.x + outsideRadius * Mathf.Sin(0), p.y + outsideRadius * Mathf.Cos(0));
                for (int j = 0; j <= indicatorNum; j++)
                {
                    float currAngle = j * angle;
                    p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle), p.y + outsideRadius * Mathf.Cos(currAngle));
                    p4 = new Vector3(p.x + insideRadius * Mathf.Sin(currAngle), p.y + insideRadius * Mathf.Cos(currAngle));

                    ChartUtils.DrawPolygon(vh, p1, p2, p3, p4, color);
                    ChartUtils.DrawLine(vh, p2, p3, radarInfo.lineTickness, radarInfo.lineColor);
                    p1 = p4;
                    p2 = p3;
                }
                insideRadius = outsideRadius;
            }
            for (int j = 0; j <= indicatorNum; j++)
            {
                float currAngle = j * angle;
                p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle), p.y + outsideRadius * Mathf.Cos(currAngle));
                ChartUtils.DrawLine(vh, p, p3, radarInfo.lineTickness/2, radarInfo.lineColor);
            }
        }

        private void UpdateRadarCenter()
        {
            float diffX = chartWid - radarInfo.left - radarInfo.right;
            float diffY = chartHig - radarInfo.top - radarInfo.bottom;
            float diff = Mathf.Min(diffX, diffY);
            if (radarInfo.radius <= 0)
            {
                radarRadius = diff / 3 * 2;
                radarCenterX = radarInfo.left + radarRadius;
                radarCenterY = radarInfo.bottom + radarRadius;
            }
            else
            {
                radarRadius = radarInfo.radius;
                radarCenterX = chartWid / 2;
                radarCenterY = chartHig / 2;
                if (radarInfo.left > 0) radarCenterX = radarInfo.left + radarRadius;
                if (radarInfo.right > 0) radarCenterX = chartWid - radarInfo.right - radarRadius;
                if (radarInfo.top > 0) radarCenterY = chartHig - radarInfo.top - radarRadius;
                if (radarInfo.bottom > 0) radarCenterY = radarInfo.bottom + radarRadius;
            }
        }
    }
}
