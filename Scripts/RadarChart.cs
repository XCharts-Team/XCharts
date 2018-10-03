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
        public bool cricle;
        public bool area;
        
        public float radius = 100;
        public int splitNumber = 5;

        public float left;
        public float right;
        public float top;
        public float bottom;

        public float lineTickness = 1f;
        public float linePointSize = 5f;
        public Color lineColor = Color.grey;
        public List<Color> backgroundColorList = new List<Color>();
        public bool showIndicator = true;
        public List<RadarIndicator> indicatorList = new List<RadarIndicator>();

        public int checkIndicatorCount { get; set; }
    }

    public class RadarChart : BaseChart
    {
        private const string INDICATOR_TEXT = "indicator";

        [SerializeField]
        private RadarInfo radarInfo = new RadarInfo();

        private RadarInfo checkRadarInfo = new RadarInfo();
        private float radarCenterX = 0f;
        private float radarCenterY = 0f;
        private float radarRadius = 0;
        private List<Text> indicatorTextList = new List<Text>();

        protected override void Awake()
        {
            base.Awake();
            UpdateRadarCenter();
        }

        protected override void Update()
        {
            base.Update();
            CheckRadarInfoChanged();
        }

        private void InitIndicator()
        {
            indicatorTextList.Clear();
            HideChild(INDICATOR_TEXT);
            int indicatorNum = radarInfo.indicatorList.Count;
            float txtWid = 100;
            float txtHig = 20;
            for (int i = 0; i < indicatorNum; i++)
            {
                var pos = GetIndicatorPosition(i);
                TextAnchor anchor = TextAnchor.MiddleCenter;
                var diff = pos.x - radarCenterX;
                if (diff < -1f)
                {
                    pos = new Vector3(pos.x - 5, pos.y);
                    anchor = TextAnchor.MiddleRight;
                }
                else if (diff > 1f)
                {
                    anchor = TextAnchor.MiddleLeft;
                    pos = new Vector3(pos.x + txtWid + 5,pos.y);
                }
                else
                {
                    anchor = TextAnchor.MiddleCenter;
                    float y = pos.y > radarCenterY ? pos.y + txtHig / 2 : pos.y - txtHig / 2;
                    pos = new Vector3(pos.x + txtWid / 2, y);
                }
                Text txt = ChartUtils.AddTextObject(INDICATOR_TEXT + i, transform, themeInfo.font,
                    themeInfo.textColor, anchor, Vector2.zero, Vector2.zero, new Vector2(1, 0.5f),
                    new Vector2(txtWid, txtHig));
                txt.transform.localPosition = pos;
                txt.text = radarInfo.indicatorList[i].name;
                txt.gameObject.SetActive(radarInfo.showIndicator);
                indicatorTextList.Add(txt);
            }
        }

        private void CheckRadarInfoChanged()
        {
            if( checkRadarInfo.radius != radarInfo.radius ||
                checkRadarInfo.left != radarInfo.left ||
                checkRadarInfo.right != radarInfo.right ||
                checkRadarInfo.top != radarInfo.top ||
                checkRadarInfo.bottom != radarInfo.bottom ||
                checkRadarInfo.checkIndicatorCount != radarInfo.indicatorList.Count ||
                checkRadarInfo.showIndicator != radarInfo.showIndicator)
            {
                checkRadarInfo.radius = radarInfo.radius;
                checkRadarInfo.left = radarInfo.left;
                checkRadarInfo.right = radarInfo.right;
                checkRadarInfo.top = radarInfo.top;
                checkRadarInfo.bottom = radarInfo.bottom;
                checkRadarInfo.showIndicator = radarInfo.showIndicator;
                checkRadarInfo.checkIndicatorCount = radarInfo.indicatorList.Count;
                OnRadarChanged();
            }
        }

        private void OnRadarChanged()
        {
            UpdateRadarCenter();
            InitIndicator();
        }

        private Vector3 GetIndicatorPosition(int i)
        {
            int indicatorNum = radarInfo.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum * i;
            var x = radarCenterX + radarInfo.radius * Mathf.Sin(angle);
            var y = radarCenterY + radarInfo.radius * Mathf.Cos(angle);

            return new Vector3(x,y);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);
            UpdateRadarCenter();
            if (radarInfo.cricle)
            {
                DrawCricleRadar(vh);
            }
            else
            {
                DrawRadar(vh);
            }
            DrawData(vh);
        }

        protected override void OnLegendButtonClicked()
        {
            base.OnLegendButtonClicked();
        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
            radarInfo.backgroundColorList.Clear();
            switch (theme)
            {
                case Theme.Dark:
                    radarInfo.backgroundColorList.Add(ThemeInfo.GetColor("#6f6f6f"));
                    radarInfo.backgroundColorList.Add(ThemeInfo.GetColor("#606060"));
                    break;
                case Theme.Default:
                    radarInfo.backgroundColorList.Add(ThemeInfo.GetColor("#f6f6f6"));
                    radarInfo.backgroundColorList.Add(ThemeInfo.GetColor("#e7e7e7"));
                    break;
                case Theme.Light:
                    radarInfo.backgroundColorList.Add(ThemeInfo.GetColor("#f6f6f6"));
                    radarInfo.backgroundColorList.Add(ThemeInfo.GetColor("#e7e7e7"));
                    break;
            }
            InitIndicator();
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
                var color = themeInfo.GetColor(i);
                var areaColor = new Color(color.r,color.g,color.b,color.a*0.7f);
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
                        if (radarInfo.area)
                        {
                            ChartUtils.DrawTriangle(vh, p, startPoint, toPoint, areaColor);
                        }
                        ChartUtils.DrawLine(vh, startPoint, toPoint, radarInfo.lineTickness, color);
                        startPoint = toPoint;
                    }
                    pointList.Add(startPoint);
                }
                if (radarInfo.area) ChartUtils.DrawTriangle(vh, p, startPoint, firstPoint, areaColor);
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
                    p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                        p.y + outsideRadius * Mathf.Cos(currAngle));
                    p4 = new Vector3(p.x + insideRadius * Mathf.Sin(currAngle),
                        p.y + insideRadius * Mathf.Cos(currAngle));

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
                p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                    p.y + outsideRadius * Mathf.Cos(currAngle));
                ChartUtils.DrawLine(vh, p, p3, radarInfo.lineTickness/2, radarInfo.lineColor);
            }
        }

        private void DrawCricleRadar(VertexHelper vh)
        {
            float insideRadius = 0, outsideRadius = 0;
            float block = radarInfo.radius / radarInfo.splitNumber;
            int indicatorNum = radarInfo.indicatorList.Count;
            Vector3 p = new Vector3(radarCenterX, radarCenterY);
            Vector3 p1;
            float angle = 2 * Mathf.PI / indicatorNum;
            for (int i = 0; i < radarInfo.splitNumber; i++)
            {
                Color color = radarInfo.backgroundColorList[i % radarInfo.backgroundColorList.Count];
                outsideRadius = insideRadius + block;
                ChartUtils.DrawDoughnut(vh, p, insideRadius, outsideRadius, 0, 360, color);
                ChartUtils.DrawCicleNotFill(vh, p, outsideRadius, radarInfo.lineTickness,
                    radarInfo.lineColor);
                insideRadius = outsideRadius;
            }
            for (int j = 0; j <= indicatorNum; j++)
            {
                float currAngle = j * angle;
                p1 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle), 
                    p.y + outsideRadius * Mathf.Cos(currAngle));
                ChartUtils.DrawLine(vh, p, p1, radarInfo.lineTickness / 2, radarInfo.lineColor);
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
