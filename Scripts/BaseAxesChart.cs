using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace xcharts
{

    [System.Serializable]
    public class Coordinate
    {
        public bool show = true;
        public float left = 40f;
        public float right = 30f;
        public float top = 40;
        public float bottom = 25f;
        public float tickness = 0.6f;
        public float splitWidth = 5.0f;
    }

    [System.Serializable]
    public enum AxisType
    {
        value,
        category,
        time,
        log
    }

    public enum SplitLineType
    {
        solid,
        dashed,
        dotted
    }

    [System.Serializable]
    public class Axis
    {
        public AxisType type;
        public int splitNumber = 5;
        public int maxSplitNumber = 5;
        public bool showSplitLine = true;
        public SplitLineType splitLineType = SplitLineType.dashed;
        public bool boundaryGap = true;
        public List<string> data;

        public void AddCategory(string category)
        {
            if (data.Count >= maxSplitNumber && maxSplitNumber != 0)
            {
                data.RemoveAt(0);
            }
            data.Add(category);
        }
    }

    [System.Serializable]
    public class XAxis : Axis
    {
    }

    [System.Serializable]
    public class YAxis : Axis
    {
    }

    public class BaseAxesChart : BaseChart
    {
        private const int DEFAULT_YSACLE_NUM = 5;
        private const string YSCALE_TEXT_PREFIX = "yScale";
        private const string XSCALE_TEXT_PREFIX = "xScale";

        [SerializeField]
        protected Coordinate coordinate = new Coordinate();
        [SerializeField]
        protected XAxis xAxis = new XAxis();
        [SerializeField]
        protected YAxis yAxis = new YAxis();

        private float lastXMaxValue;
        private float lastYMaxValue;
        private float lastCoordinateWid;
        private float lastCoordinateHig;
        private float lastCoordinateScaleLen;

        private XAxis checkXAxis = new XAxis();
        private YAxis checkYAxis = new YAxis();
        private Coordinate checkCoordinate = new Coordinate();

        protected List<Text> yScaleTextList = new List<Text>();
        protected List<Text> xScaleTextList = new List<Text>();
        protected float zeroX { get { return coordinate.left; } }
        protected float zeroY { get { return coordinate.bottom; } }
        protected float coordinateWid { get { return chartWid - coordinate.left - coordinate.right; } }
        protected float coordinateHig { get { return chartHig - coordinate.top - coordinate.bottom; } }

        protected override void Awake()
        {
            base.Awake();
            lastCoordinateHig = chartHig;
            lastCoordinateWid = chartWid;
            lastCoordinateScaleLen = coordinate.splitWidth;
            InitXScale();
            InitYScale();
        }

        protected override void Update()
        {
            base.Update();
            CheckYAxisType();
            CheckXAxisType();
            CheckMaxValue();
            CheckCoordinate();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);
            DrawCoordinate(vh);
        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
            InitXScale();
            InitYScale();
        }

        public void AddXAxisCategory(string category)
        {
            xAxis.AddCategory(category);
            OnXAxisChanged();
        }

        public void AddYAxisCategory(string category)
        {
            yAxis.AddCategory(category);
            OnYAxisChanged();
        }

        private void InitYScale()
        {
            yScaleTextList.Clear();
            if (yAxis.type == AxisType.value)
            {
                float max = GetMaxValue();
                if (max <= 0) max = 400;
                yAxis.splitNumber = DEFAULT_YSACLE_NUM;
                for (int i = 0; i < yAxis.splitNumber; i++)
                {
                    Text txt = ChartUtils.AddTextObject(YSCALE_TEXT_PREFIX + i, transform, themeInfo.font,
                        themeInfo.textColor, TextAnchor.MiddleRight, Vector2.zero, Vector2.zero, 
                        new Vector2(1, 0.5f),
                        new Vector2(coordinate.left, 20));
                    txt.transform.localPosition = GetYScalePosition(i);
                    txt.text = ((int)(max * i / yAxis.splitNumber)).ToString();
                    txt.gameObject.SetActive(coordinate.show);
                    yScaleTextList.Add(txt);
                }
            }
            else
            {
                yAxis.splitNumber = yAxis.boundaryGap ? yAxis.data.Count + 1 : yAxis.data.Count;
                for (int i = 0; i < yAxis.data.Count; i++)
                {
                    Text txt = ChartUtils.AddTextObject(YSCALE_TEXT_PREFIX + i, transform, themeInfo.font,
                        themeInfo.textColor, TextAnchor.MiddleRight, Vector2.zero, Vector2.zero,
                        new Vector2(1, 0.5f),
                        new Vector2(coordinate.left, 20));
                    txt.transform.localPosition = GetYScalePosition(i);
                    txt.text = yAxis.data[i];
                    txt.gameObject.SetActive(coordinate.show);
                    yScaleTextList.Add(txt);
                }
            }
        }

        private void InitXScale()
        {
            xScaleTextList.Clear();
            if (xAxis.type == AxisType.value)
            {
                float max = GetMaxValue();
                if (max <= 0) max = 400;
                xAxis.splitNumber = DEFAULT_YSACLE_NUM;
                float scaleWid = coordinateWid / (xAxis.splitNumber - 1);
                for (int i = 0; i < xAxis.splitNumber; i++)
                {
                    Text txt = ChartUtils.AddTextObject(XSCALE_TEXT_PREFIX + i, transform, themeInfo.font,
                        themeInfo.textColor, TextAnchor.MiddleCenter, Vector2.zero, Vector2.zero,
                        new Vector2(1, 0.5f),
                        new Vector2(scaleWid, 20));
                    txt.transform.localPosition = GetXScalePosition(i);
                    txt.text = ((int)(max * i / xAxis.splitNumber)).ToString();
                    txt.gameObject.SetActive(coordinate.show);
                    xScaleTextList.Add(txt);
                }
            }
            else
            {
                xAxis.splitNumber = xAxis.boundaryGap ? xAxis.data.Count + 1 : xAxis.data.Count;
                float scaleWid = coordinateWid / (xAxis.data.Count - 1);
                for (int i = 0; i < xAxis.data.Count; i++)
                {
                    Text txt = ChartUtils.AddTextObject(XSCALE_TEXT_PREFIX + i, transform, themeInfo.font,
                        themeInfo.textColor, TextAnchor.MiddleCenter, Vector2.zero, Vector2.zero,
                        new Vector2(1, 0.5f),
                        new Vector2(scaleWid, 20));
                    txt.transform.localPosition = GetXScalePosition(i);
                    txt.text = xAxis.data[i];
                    txt.gameObject.SetActive(coordinate.show);
                    xScaleTextList.Add(txt);
                }
            }
        }

        private Vector3 GetYScalePosition(int i)
        {
            float scaleWid = coordinateHig / (yAxis.splitNumber - 1);
            if (yAxis.type == AxisType.value)
            {
                return new Vector3(zeroX - coordinate.splitWidth - 2f,
                zeroY + i * scaleWid, 0);
            }
            else
            {
                if (yAxis.boundaryGap)
                {
                    return new Vector3(zeroX - coordinate.splitWidth - 2f,
                        zeroY + (i + 0.5f) * scaleWid, 0);
                }
                else
                {
                    return new Vector3(zeroX - coordinate.splitWidth - 2f,
                        zeroY + i * scaleWid, 0);
                }

            }
        }

        private Vector3 GetXScalePosition(int i)
        {
            float scaleWid = coordinateWid / (xAxis.splitNumber - 1);
            if (xAxis.type == AxisType.value)
            {
                return new Vector3(zeroX + (i + 1 - 0.5f) * scaleWid,
                    zeroY - coordinate.splitWidth - 10, 0);
            }
            else
            {
                if (xAxis.boundaryGap)
                {
                    return new Vector3(zeroX + (i + 1) * scaleWid, zeroY - coordinate.splitWidth - 5, 0);
                }
                else
                {
                    return new Vector3(zeroX + (i + 1 - 0.5f) * scaleWid,
                        zeroY - coordinate.splitWidth - 5, 0);
                }
            }
        }

        private void CheckCoordinate()
        {
            if (lastCoordinateHig != coordinateHig
                || lastCoordinateWid != coordinateWid
                || lastCoordinateScaleLen != coordinate.splitWidth)
            {
                lastCoordinateWid = coordinateWid;
                lastCoordinateHig = coordinateHig;
                lastCoordinateScaleLen = coordinate.splitWidth;
                OnCoordinateSize();
            }
            if (checkCoordinate.show != coordinate.show)
            {
                checkCoordinate.show = coordinate.show;
                OnXAxisChanged();
                OnYAxisChanged();
            }
        }

        private void CheckYAxisType()
        {
            if (checkYAxis.type != yAxis.type ||
                checkYAxis.boundaryGap != yAxis.boundaryGap ||
                checkYAxis.showSplitLine != yAxis.showSplitLine ||
                checkYAxis.splitNumber != yAxis.splitNumber)
            {
                checkYAxis.type = yAxis.type;
                checkYAxis.boundaryGap = yAxis.boundaryGap;
                checkYAxis.showSplitLine = yAxis.showSplitLine;
                checkYAxis.splitNumber = yAxis.splitNumber;
                OnYAxisChanged();
            }
        }

        private void CheckXAxisType()
        {
            if (checkXAxis.type != xAxis.type ||
                checkXAxis.boundaryGap != xAxis.boundaryGap ||
                checkXAxis.showSplitLine != xAxis.showSplitLine ||
                checkXAxis.splitNumber != xAxis.splitNumber)
            {
                checkXAxis.type = xAxis.type;
                checkXAxis.boundaryGap = xAxis.boundaryGap;
                checkXAxis.showSplitLine = xAxis.showSplitLine;
                checkXAxis.splitNumber = xAxis.splitNumber;
                OnXAxisChanged();
            }
        }

        private void CheckMaxValue()
        {
            if (xAxis.type == AxisType.value)
            {
                float max = GetMaxValue();
                if (lastXMaxValue != max)
                {
                    lastXMaxValue = max;
                    OnXMaxValueChanged();
                }
            }
            else if (yAxis.type == AxisType.value)
            {

                float max = GetMaxValue();
                if (lastYMaxValue != max)
                {
                    lastYMaxValue = max;
                    OnYMaxValueChanged();
                }
            }
        }

        protected virtual void OnCoordinateSize()
        {
            //update yScale pos
            for (int i = 0; i < yAxis.splitNumber; i++)
            {
                if (i < yScaleTextList.Count && yScaleTextList[i])
                {
                    yScaleTextList[i].transform.localPosition = GetYScalePosition(i);
                }
            }
            for (int i = 0; i < xAxis.splitNumber; i++)
            {
                if (i < xScaleTextList.Count && xScaleTextList[i])
                {
                    xScaleTextList[i].transform.localPosition = GetXScalePosition(i);
                }
            }
        }

        protected virtual void OnYAxisChanged()
        {
            HideChild(YSCALE_TEXT_PREFIX);
            InitYScale();
        }

        protected virtual void OnXAxisChanged()
        {
            HideChild(XSCALE_TEXT_PREFIX);
            InitXScale();
        }

        protected virtual void OnXMaxValueChanged()
        {
            float max = GetMaxValue();
            for (int i = 0; i < xScaleTextList.Count; i++)
            {
                xScaleTextList[i].text = ((int)(max * i / xScaleTextList.Count)).ToString();
            }
        }

        protected override void OnYMaxValueChanged()
        {
            float max = GetMaxValue();
            for (int i = 0; i < yScaleTextList.Count; i++)
            {
                yScaleTextList[i].text = ((int)(max * i / (yScaleTextList.Count - 1))).ToString();
            }
        }

        private void DrawCoordinate(VertexHelper vh)
        {
            if (!coordinate.show) return;
            // draw splitline
            for (int i = 1; i < yAxis.splitNumber; i++)
            {
                float pX = zeroX - coordinate.splitWidth;
                float pY = zeroY + i * coordinateHig / (yAxis.splitNumber - 1);
                ChartUtils.DrawLine(vh, new Vector3(pX, pY), new Vector3(zeroX, pY), coordinate.tickness,
                    themeInfo.axisLineColor);
                if (yAxis.showSplitLine)
                {
                    DrawSplitLine(vh, true,yAxis.splitLineType, new Vector3(zeroX, pY),
                        new Vector3(zeroX + coordinateWid, pY));
                }
            }
            for (int i = 1; i < xAxis.splitNumber; i++)
            {
                float pX = zeroX + i * coordinateWid / (xAxis.splitNumber - 1);
                float pY = zeroY - coordinate.splitWidth - 2;
                ChartUtils.DrawLine(vh, new Vector3(pX, zeroY), new Vector3(pX, pY), coordinate.tickness,
                    themeInfo.axisLineColor);
                if (xAxis.showSplitLine)
                {
                    DrawSplitLine(vh, false,xAxis.splitLineType, new Vector3(pX, zeroY),
                        new Vector3(pX, zeroY + coordinateHig));
                }
            }
            //draw x,y axis
            ChartUtils.DrawLine(vh, new Vector3(zeroX, zeroY - coordinate.splitWidth),
                new Vector3(zeroX, zeroY + coordinateHig + 2), coordinate.tickness, 
                themeInfo.axisLineColor);
            ChartUtils.DrawLine(vh, new Vector3(zeroX - coordinate.splitWidth, zeroY),
                new Vector3(zeroX + coordinateWid + 2, zeroY), coordinate.tickness, 
                themeInfo.axisLineColor);
        }

        private void DrawSplitLine(VertexHelper vh,bool isYAxis,SplitLineType type,Vector3 startPos,
            Vector3 endPos)
        {
            switch (type)
            {
                case SplitLineType.dashed:
                case SplitLineType.dotted:
                    var startX = startPos.x;
                    var startY = startPos.y;
                    var dashLen = type == SplitLineType.dashed ? 6 : 2.5f;
                    var count = isYAxis ? (endPos.x - startPos.x) / (dashLen * 2):
                        (endPos.y - startPos.y) / (dashLen * 2);
                    for (int i = 0; i < count; i++)
                    {
                        if (isYAxis)
                        {
                            var toX = startX + dashLen;
                            ChartUtils.DrawLine(vh, new Vector3(startX, startY), new Vector3(toX, startY),
                                coordinate.tickness, themeInfo.axisSplitLineColor);
                            startX += dashLen * 2;
                        }
                        else
                        {
                            var toY = startY + dashLen;
                            ChartUtils.DrawLine(vh, new Vector3(startX, startY), new Vector3(startX, toY),
                                coordinate.tickness, themeInfo.axisSplitLineColor);
                            startY += dashLen * 2;
                        }
                        
                    }
                    break;
                case SplitLineType.solid:
                    ChartUtils.DrawLine(vh, startPos, endPos, coordinate.tickness,
                        themeInfo.axisSplitLineColor);
                    break;
            }
        }
    }
}

