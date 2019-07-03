using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;

namespace XCharts
{
    public class CoordinateChart : BaseChart
    {
        private static readonly string s_DefaultAxisY = "axis_y";
        private static readonly string s_DefaultAxisX = "axis_x";
        private static readonly string s_DefaultDataZoom = "datazoom";

        [SerializeField] protected Coordinate m_Coordinate = Coordinate.defaultCoordinate;
        [SerializeField] protected XAxis m_XAxis = XAxis.defaultXAxis;
        [SerializeField] protected YAxis m_YAxis = YAxis.defaultYAxis;
        [SerializeField] protected DataZoom m_DataZoom = DataZoom.defaultDataZoom;

        private float m_ZeroXOffset;
        private float m_ZeroYOffset;
        private bool m_DataZoomDrag;
        private bool m_DataZoomStartDrag;
        private bool m_DataZoomEndDrag;
        private float m_DataZoomLastStartIndex;
        private float m_DataZoomLastEndIndex;

        private XAxis m_CheckXAxis = XAxis.defaultXAxis;
        private YAxis m_CheckYAxis = YAxis.defaultYAxis;
        private Coordinate m_CheckCoordinate = Coordinate.defaultCoordinate;
        private List<Text> m_AxisLabelYTextList = new List<Text>();
        private List<Text> m_SplitXTextList = new List<Text>();

        public int minValue { get; private set; }
        public int maxValue { get; private set; }
        public float zeroX { get { return coordinateX + m_ZeroXOffset; } }
        public float zeroY { get { return coordinateY + m_ZeroYOffset; } }
        public float coordinateX { get { return m_Coordinate.left; } }
        public float coordinateY { get { return m_Coordinate.bottom; } }
        public float coordinateWid { get { return chartWidth - m_Coordinate.left - m_Coordinate.right; } }
        public float coordinateHig { get { return chartHeight - m_Coordinate.top - m_Coordinate.bottom; } }
        public Axis xAxis { get { return m_XAxis; } }
        public Axis yAxis { get { return m_YAxis; } }

        protected override void Awake()
        {
            base.Awake();
            CheckMinMaxValue();
            InitDataZoom();
            InitAxisX();
            InitAxisY();
        }

        protected override void Update()
        {
            base.Update();
            CheckYAxis();
            CheckXAxis();
            CheckMinMaxValue();
            CheckCoordinate();
            CheckDataZoom();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Coordinate = Coordinate.defaultCoordinate;
            m_XAxis = XAxis.defaultXAxis;
            m_YAxis = YAxis.defaultYAxis;
            InitAxisX();
            InitAxisY();
        }
#endif

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            DrawCoordinate(vh);
            DrawDataZoom(vh);
        }

        protected override void CheckTootipArea(Vector2 local)
        {
            if (local.x < coordinateX || local.x > coordinateX + coordinateWid ||
                local.y < coordinateY || local.y > coordinateY + coordinateHig)
            {
                m_Tooltip.dataIndex = 0;
                RefreshTooltip();
            }
            else
            {
                if (m_XAxis.type == Axis.AxisType.Value)
                {
                    float splitWid = m_YAxis.GetDataWidth(coordinateHig, m_DataZoom);
                    for (int i = 0; i < m_YAxis.GetDataNumber(m_DataZoom); i++)
                    {
                        float pY = zeroY + i * splitWid;
                        if (m_YAxis.boundaryGap)
                        {
                            if (local.y > pY && local.y <= pY + splitWid)
                            {
                                m_Tooltip.dataIndex = i + 1;
                                break;
                            }
                        }
                        else
                        {
                            if (local.y > pY - splitWid / 2 && local.y <= pY + splitWid / 2)
                            {
                                m_Tooltip.dataIndex = i + 1;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    float splitWid = m_XAxis.GetDataWidth(coordinateWid, m_DataZoom);
                    for (int i = 0; i < m_XAxis.GetDataNumber(m_DataZoom); i++)
                    {
                        float pX = zeroX + i * splitWid;
                        if (m_XAxis.boundaryGap)
                        {
                            if (local.x > pX && local.x <= pX + splitWid)
                            {
                                m_Tooltip.dataIndex = i + 1;
                                break;
                            }
                        }
                        else
                        {
                            if (local.x > pX - splitWid / 2 && local.x <= pX + splitWid / 2)
                            {
                                m_Tooltip.dataIndex = i + 1;
                                break;
                            }
                        }
                    }
                }
            }
            if (m_Tooltip.dataIndex > 0)
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                RefreshTooltip();
                if (m_Tooltip.lastDataIndex != m_Tooltip.dataIndex || m_Tooltip.crossLabel)
                {
                    RefreshChart();
                }
                m_Tooltip.lastDataIndex = m_Tooltip.dataIndex;
            }
        }

        protected override void RefreshTooltip()
        {
            base.RefreshTooltip();
            int index = m_Tooltip.dataIndex - 1;
            Axis tempAxis = m_XAxis.type == Axis.AxisType.Value ? (Axis)m_YAxis : (Axis)m_XAxis;
            if (index < 0)
            {
                m_Tooltip.SetActive(false);
                return;
            }
            if (m_Series.Count == 1)
            {
                float value = m_Series.GetData(0, index);
                string txt = tempAxis.GetData(index, m_DataZoom) + ": " + value;
                m_Tooltip.UpdateContentText(txt);
            }
            else
            {
                StringBuilder sb = new StringBuilder(tempAxis.GetData(index, m_DataZoom));
                for (int i = 0; i < m_Series.Count; i++)
                {
                    if (m_Series.series[i].show)
                    {
                        string strColor = ColorUtility.ToHtmlStringRGBA(m_ThemeInfo.GetColor(i));
                        string key = m_Series.series[i].name;
                        float value = m_Series.series[i].GetData(index, m_DataZoom);
                        sb.Append("\n");
                        sb.AppendFormat("<color=#{0}>● </color>", strColor);
                        sb.AppendFormat("{0}: {1}", key, value);
                    }

                }
                m_Tooltip.UpdateContentText(sb.ToString());
            }
            if (m_XAxis.type == Axis.AxisType.Value)
            {
                float hig = (maxValue - minValue) * (m_Tooltip.pointerPos.x - zeroX) / coordinateWid;
                m_Tooltip.UpdateLabelText(hig.ToString("f2"), tempAxis.GetData(index, m_DataZoom));
                float splitWidth = m_YAxis.GetSplitWidth(coordinateHig, m_DataZoom);
                float py = zeroY + (m_Tooltip.dataIndex - 1) * splitWidth
                    + (m_YAxis.boundaryGap ? splitWidth / 2 : 0);
                Vector2 xLabelPos = new Vector2(m_Tooltip.pointerPos.x, coordinateY - 4 * m_Coordinate.tickness);
                Vector2 yLabelPos = new Vector2(coordinateX - 6 * m_Coordinate.tickness, py);
                m_Tooltip.UpdateLabelPos(xLabelPos, yLabelPos);
            }
            else
            {
                float hig = (maxValue - minValue) * (m_Tooltip.pointerPos.y - zeroY) / coordinateHig;
                m_Tooltip.UpdateLabelText(tempAxis.GetData(index, m_DataZoom), hig.ToString("f2"));
                float splitWidth = m_XAxis.GetSplitWidth(coordinateWid, m_DataZoom);
                float px = zeroX + (m_Tooltip.dataIndex - 1) * splitWidth
                    + (m_XAxis.boundaryGap ? splitWidth / 2 : 0);
                Vector2 xLabelPos = new Vector2(px, coordinateY - 6 * m_Coordinate.tickness);
                Vector2 yLabelPos = new Vector2(coordinateX - 4 * m_Coordinate.tickness, m_Tooltip.pointerPos.y);
                m_Tooltip.UpdateLabelPos(xLabelPos, yLabelPos);
            }


            var pos = m_Tooltip.GetContentPos();
            if (pos.x + m_Tooltip.width > chartWidth)
            {
                pos.x = chartWidth - m_Tooltip.width;
            }
            if (pos.y - m_Tooltip.height < 0)
            {
                pos.y = m_Tooltip.height;
            }
            m_Tooltip.UpdateContentPos(pos);
            m_Tooltip.SetActive(true);
        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
            InitDataZoom();
            InitAxisX();
            InitAxisY();
        }

        public void AddXAxisData(string category)
        {
            m_XAxis.AddData(category, m_MaxCacheDataNumber);
            OnXAxisChanged();
        }

        public void AddYAxisData(string category)
        {
            m_YAxis.AddData(category, m_MaxCacheDataNumber);
            OnYAxisChanged();
        }

        private void InitAxisY()
        {
            m_AxisLabelYTextList.Clear();
            ChartHelper.HideAllObject(gameObject, "split_y");//old name
            float labelWidth = m_YAxis.GetScaleWidth(coordinateHig, m_DataZoom);

            var axisObj = ChartHelper.AddObject(s_DefaultAxisY, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(m_YAxis.axisLabel.show);
            ChartHelper.HideAllObject(axisObj, s_DefaultAxisY);

            var labelColor = m_YAxis.axisLabel.color == Color.clear ?
                (Color)m_ThemeInfo.axisTextColor :
                m_YAxis.axisLabel.color;
            for (int i = 0; i < m_YAxis.GetSplitNumber(m_DataZoom); i++)
            {
                Text txt;
                if (m_YAxis.axisLabel.inside)
                {
                    txt = ChartHelper.AddTextObject(s_DefaultAxisY + i, axisObj.transform,
                        m_ThemeInfo.font, labelColor, TextAnchor.MiddleLeft, Vector2.zero,
                        Vector2.zero, new Vector2(0, 0.5f), new Vector2(m_Coordinate.left, 20),
                        m_YAxis.axisLabel.fontSize, m_YAxis.axisLabel.rotate, m_YAxis.axisLabel.fontStyle);
                }
                else
                {
                    txt = ChartHelper.AddTextObject(s_DefaultAxisY + i, axisObj.transform,
                        m_ThemeInfo.font, labelColor, TextAnchor.MiddleRight, Vector2.zero,
                        Vector2.zero, new Vector2(1, 0.5f), new Vector2(m_Coordinate.left, 20),
                        m_YAxis.axisLabel.fontSize, m_YAxis.axisLabel.rotate, m_YAxis.axisLabel.fontStyle);
                }

                txt.transform.localPosition = GetLabelYPosition(labelWidth, i, m_YAxis.axisLabel.inside);
                txt.text = m_YAxis.GetScaleName(i, minValue, maxValue, m_DataZoom);
                txt.gameObject.SetActive(m_YAxis.show &&
                    (m_YAxis.axisLabel.interval == 0 || i % (m_YAxis.axisLabel.interval + 1) == 0));
                m_AxisLabelYTextList.Add(txt);
            }
            if (m_YAxis.axisName.show)
            {
                var color = m_YAxis.axisName.color == Color.clear ? (Color)m_ThemeInfo.axisTextColor :
                    m_YAxis.axisName.color;
                var fontSize = m_YAxis.axisName.fontSize;
                var gap = m_YAxis.axisName.gap;
                Text axisName;
                switch (m_YAxis.axisName.location)
                {
                    case Axis.AxisName.Location.Start:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisX + "_name", axisObj.transform,
                             m_ThemeInfo.font, color, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f),
                             new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 20), fontSize,
                             m_YAxis.axisName.rotate, m_YAxis.axisName.fontStyle);
                        axisName.transform.localPosition = new Vector2(coordinateX, coordinateY - gap);
                        break;
                    case Axis.AxisName.Location.Middle:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisX + "_name", axisObj.transform,
                            m_ThemeInfo.font, color, TextAnchor.MiddleRight, new Vector2(1, 0.5f),
                            new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(100, 20), fontSize,
                            m_YAxis.axisName.rotate, m_YAxis.axisName.fontStyle);
                        axisName.transform.localPosition = new Vector2(coordinateX - gap,
                        coordinateY + coordinateHig / 2);
                        break;
                    case Axis.AxisName.Location.End:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisX + "_name", axisObj.transform,
                             m_ThemeInfo.font, color, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f),
                             new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 20), fontSize,
                             m_YAxis.axisName.rotate, m_YAxis.axisName.fontStyle);
                        axisName.transform.localPosition = new Vector2(coordinateX,
                            coordinateY + coordinateHig + gap);
                        break;
                }
            }
        }

        private void InitAxisX()
        {
            m_SplitXTextList.Clear();
            ChartHelper.HideAllObject(gameObject, "split_x");//old name
            float labelWidth = m_XAxis.GetScaleWidth(coordinateWid, m_DataZoom);

            var axisObj = ChartHelper.AddObject(s_DefaultAxisX, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(m_XAxis.axisLabel.show);
            ChartHelper.HideAllObject(axisObj, s_DefaultAxisX);
            var labelColor = m_XAxis.axisLabel.color == Color.clear ?
                (Color)m_ThemeInfo.axisTextColor :
                m_XAxis.axisLabel.color;
            for (int i = 0; i < m_XAxis.GetSplitNumber(m_DataZoom); i++)
            {
                Text txt = ChartHelper.AddTextObject(s_DefaultAxisX + i, axisObj.transform,
                    m_ThemeInfo.font, labelColor, TextAnchor.MiddleCenter, new Vector2(0, 1),
                    new Vector2(0, 1), new Vector2(1, 0.5f), new Vector2(labelWidth, 20),
                    m_XAxis.axisLabel.fontSize, m_XAxis.axisLabel.rotate, m_XAxis.axisLabel.fontStyle);

                txt.transform.localPosition = GetLabelXPosition(labelWidth, i, m_XAxis.axisLabel.inside);
                txt.text = m_XAxis.GetScaleName(i, minValue, maxValue, m_DataZoom);
                txt.gameObject.SetActive(m_XAxis.show &&
                (m_XAxis.axisLabel.interval == 0 || i % (m_XAxis.axisLabel.interval + 1) == 0));
                m_SplitXTextList.Add(txt);
            }
            if (m_XAxis.axisName.show)
            {
                var color = m_XAxis.axisName.color == Color.clear ? (Color)m_ThemeInfo.axisTextColor :
                    m_XAxis.axisName.color;
                var fontSize = m_XAxis.axisName.fontSize;
                var gap = m_XAxis.axisName.gap;
                Text axisName;
                switch (m_XAxis.axisName.location)
                {
                    case Axis.AxisName.Location.Start:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisX + "_name", axisObj.transform,
                            m_ThemeInfo.font, color, TextAnchor.MiddleRight, new Vector2(1, 0.5f),
                            new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(100, 20), fontSize,
                            m_XAxis.axisName.rotate, m_XAxis.axisName.fontStyle);
                        axisName.transform.localPosition = new Vector2(coordinateX - gap, coordinateY);
                        break;
                    case Axis.AxisName.Location.Middle:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisX + "_name", axisObj.transform,
                             m_ThemeInfo.font, color, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f),
                             new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 20), fontSize,
                             m_XAxis.axisName.rotate, m_XAxis.axisName.fontStyle);
                        axisName.transform.localPosition = new Vector2(coordinateX + coordinateWid / 2,
                            coordinateY - gap);
                        break;
                    case Axis.AxisName.Location.End:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisX + "_name", axisObj.transform,
                             m_ThemeInfo.font, color, TextAnchor.MiddleLeft, new Vector2(0, 0.5f),
                             new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(100, 20), fontSize,
                             m_XAxis.axisName.rotate, m_XAxis.axisName.fontStyle);
                        axisName.transform.localPosition = new Vector2(coordinateX + coordinateWid + gap,
                            coordinateY);
                        break;
                }
            }
        }

        private void InitDataZoom()
        {
            var dataZoomObject = ChartHelper.AddObject(s_DefaultDataZoom, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            dataZoomObject.transform.localPosition = Vector3.zero;
            ChartHelper.HideAllObject(dataZoomObject, s_DefaultDataZoom);
            m_DataZoom.startLabel = ChartHelper.AddTextObject(s_DefaultDataZoom + "start",
                dataZoomObject.transform, m_ThemeInfo.font, m_ThemeInfo.dataZoomTextColor, TextAnchor.MiddleRight,
                Vector2.zero, Vector2.zero, new Vector2(1, 0.5f), new Vector2(200, 20));
            m_DataZoom.endLabel = ChartHelper.AddTextObject(s_DefaultDataZoom + "end",
                dataZoomObject.transform, m_ThemeInfo.font, m_ThemeInfo.dataZoomTextColor, TextAnchor.MiddleLeft,
                Vector2.zero, Vector2.zero, new Vector2(0, 0.5f), new Vector2(200, 20));
            m_DataZoom.SetLabelActive(false);
            if (m_XAxis != null)
            {
                m_XAxis.UpdateFilterData(m_DataZoom);
            }
            if (m_Series != null)
            {
                m_Series.UpdateFilterData(m_DataZoom);
            }
            raycastTarget = m_DataZoom.show;
        }

        private Vector3 GetLabelYPosition(float scaleWid, int i, bool inside)
        {
            var posX = inside ?
                coordinateX + m_YAxis.axisLabel.margin :
                coordinateX - m_YAxis.axisLabel.margin;
            if (m_YAxis.boundaryGap)
            {
                return new Vector3(posX, coordinateY + (i + 0.5f) * scaleWid, 0);
            }
            else
            {
                return new Vector3(posX, coordinateY + i * scaleWid, 0);
            }
        }

        private Vector3 GetLabelXPosition(float scaleWid, int i, bool inside)
        {
            var posY = inside ?
                coordinateY + m_XAxis.axisLabel.margin + m_XAxis.axisLabel.fontSize / 2 :
                coordinateY - m_XAxis.axisLabel.margin - m_XAxis.axisLabel.fontSize / 2;
            if (m_XAxis.boundaryGap)
            {
                return new Vector3(coordinateX + (i + 1) * scaleWid, posY);
            }
            else
            {
                return new Vector3(coordinateX + (i + 1 - 0.5f) * scaleWid, posY);
            }
        }

        private void CheckCoordinate()
        {
            if (m_CheckCoordinate != m_Coordinate)
            {
                m_CheckCoordinate.Copy(m_Coordinate);
                OnCoordinateChanged();
            }
        }

        private void CheckYAxis()
        {
            if (m_CheckYAxis != m_YAxis)
            {
                m_CheckYAxis.Copy(m_YAxis);
                OnYAxisChanged();
            }
        }

        private void CheckXAxis()
        {
            if (m_CheckXAxis != m_XAxis)
            {
                m_CheckXAxis.Copy(m_XAxis);
                OnXAxisChanged();
            }
        }

        private void CheckMinMaxValue()
        {
            int tempMinValue = 0;
            int tempMaxValue = 100;
            if (m_Series != null)
            {
                m_Series.GetMinMaxValue(m_DataZoom, out tempMinValue, out tempMaxValue);
            }
            Axis axis;
            if (m_XAxis.type == Axis.AxisType.Value) axis = m_XAxis;
            else axis = m_YAxis;
            switch (axis.minMaxType)
            {
                case Axis.AxisMinMaxType.Default:
                    if (tempMinValue > 0 && tempMaxValue > 0)
                    {
                        tempMinValue = 0;
                        tempMaxValue = ChartHelper.GetMaxDivisibleValue(tempMaxValue);
                    }
                    else if (tempMinValue < 0 && tempMaxValue < 0)
                    {
                        tempMinValue = ChartHelper.GetMinDivisibleValue(tempMinValue);
                        tempMaxValue = 0;
                    }
                    else
                    {
                        tempMinValue = ChartHelper.GetMinDivisibleValue(tempMinValue);
                        tempMaxValue = ChartHelper.GetMaxDivisibleValue(tempMaxValue);
                    }
                    break;
                case Axis.AxisMinMaxType.MinMax:
                    tempMinValue = ChartHelper.GetMinDivisibleValue(tempMinValue);
                    tempMaxValue = ChartHelper.GetMaxDivisibleValue(tempMaxValue);
                    break;
                case Axis.AxisMinMaxType.Custom:
                    if (axis.min != 0 || axis.max != 0)
                    {
                        tempMinValue = axis.min;
                        tempMaxValue = axis.max;
                    }
                    break;
            }
            if (tempMinValue != minValue || tempMaxValue != maxValue)
            {
                minValue = tempMinValue;
                maxValue = tempMaxValue;
                if (m_XAxis.type == Axis.AxisType.Value)
                {
                    m_ZeroXOffset = minValue > 0 ? 0 :
                        maxValue < 0 ? coordinateWid :
                        Mathf.Abs(minValue) * (coordinateWid / (Mathf.Abs(minValue) + Mathf.Abs(maxValue)));
                    OnXMaxValueChanged();
                }
                else if (m_YAxis.type == Axis.AxisType.Value)
                {
                    m_ZeroYOffset = minValue > 0 ? 0 :
                        maxValue < 0 ? coordinateHig :
                        Mathf.Abs(minValue) * (coordinateHig / (Mathf.Abs(minValue) + Mathf.Abs(maxValue)));
                    OnYMaxValueChanged();
                }
                RefreshChart();
            }
        }

        protected virtual void OnCoordinateChanged()
        {
            InitAxisX();
            InitAxisY();
        }

        protected virtual void OnYAxisChanged()
        {
            InitAxisY();
        }

        protected virtual void OnXAxisChanged()
        {
            InitAxisX();
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            minValue = 0;
            maxValue = 100;
            InitAxisX();
            InitAxisY();
        }

        protected override void OnYMaxValueChanged()
        {
            for (int i = 0; i < m_AxisLabelYTextList.Count; i++)
            {
                m_AxisLabelYTextList[i].text = m_YAxis.GetScaleName(i, minValue, maxValue, m_DataZoom);
            }
        }

        protected virtual void OnXMaxValueChanged()
        {
            for (int i = 0; i < m_SplitXTextList.Count; i++)
            {
                m_SplitXTextList[i].text = m_XAxis.GetScaleName(i, minValue, maxValue, m_DataZoom);
            }
        }

        private void DrawCoordinate(VertexHelper vh)
        {
            #region draw tick and splitline
            if (m_YAxis.show)
            {
                var scaleWidth = m_YAxis.GetScaleWidth(coordinateHig, m_DataZoom);
                var size = m_YAxis.GetScaleNumber(m_DataZoom);
                for (int i = 0; i < size; i++)
                {
                    float pX = 0;
                    float pY = coordinateY + i * scaleWidth;
                    if (m_YAxis.boundaryGap && m_YAxis.axisTick.alignWithLabel)
                    {
                        pY -= scaleWidth / 2;
                    }
                    if (m_YAxis.splitArea.show && i < size - 1)
                    {
                        ChartHelper.DrawPolygon(vh, new Vector2(coordinateX, pY),
                            new Vector2(coordinateX + coordinateWid, pY),
                            new Vector2(coordinateX + coordinateWid, pY + scaleWidth),
                            new Vector2(coordinateX, pY + scaleWidth),
                            m_YAxis.splitArea.getColor(i));
                    }
                    if (m_YAxis.axisTick.show)
                    {
                        pX += zeroX - (m_YAxis.axisTick.inside ? -m_YAxis.axisTick.length :
                            m_YAxis.axisTick.length);
                        ChartHelper.DrawLine(vh, new Vector3(zeroX, pY), new Vector3(pX, pY),
                            m_Coordinate.tickness, m_ThemeInfo.axisLineColor);
                    }
                    if (m_YAxis.showSplitLine)
                    {
                        DrawSplitLine(vh, true, m_YAxis.splitLineType, new Vector3(coordinateX, pY),
                            new Vector3(coordinateX + coordinateWid, pY), m_ThemeInfo.axisSplitLineColor);
                    }
                }
            }
            if (m_XAxis.show)
            {
                var scaleWidth = m_XAxis.GetScaleWidth(coordinateWid, m_DataZoom);
                var size = m_XAxis.GetScaleNumber(m_DataZoom);
                for (int i = 0; i < size; i++)
                {
                    float pX = coordinateX + i * scaleWidth;
                    float pY = 0;
                    if (m_XAxis.boundaryGap && m_XAxis.axisTick.alignWithLabel)
                    {
                        pX -= scaleWidth / 2;
                    }
                    if (m_XAxis.splitArea.show && i < size - 1)
                    {
                        ChartHelper.DrawPolygon(vh, new Vector2(pX, coordinateY),
                            new Vector2(pX, coordinateY + coordinateHig),
                            new Vector2(pX + scaleWidth, coordinateY + coordinateHig),
                            new Vector2(pX + scaleWidth, coordinateY),
                            m_XAxis.splitArea.getColor(i));
                    }
                    if (m_XAxis.axisTick.show)
                    {
                        pY += zeroY + (m_XAxis.axisTick.inside ? m_XAxis.axisTick.length :
                            -m_XAxis.axisTick.length);
                        ChartHelper.DrawLine(vh, new Vector3(pX, zeroY), new Vector3(pX, pY),
                            m_Coordinate.tickness, m_ThemeInfo.axisLineColor);
                    }
                    if (m_XAxis.showSplitLine)
                    {
                        DrawSplitLine(vh, false, m_XAxis.splitLineType, new Vector3(pX, coordinateY),
                            new Vector3(pX, coordinateY + coordinateHig), m_ThemeInfo.axisSplitLineColor);
                    }
                }
            }
            #endregion

            #region draw x,y axis
            if (m_YAxis.show)
            {
                if (m_YAxis.type == Axis.AxisType.Value)
                {
                    ChartHelper.DrawLine(vh, new Vector3(coordinateX, coordinateY - m_Coordinate.tickness),
                        new Vector3(coordinateX, coordinateY + coordinateHig + m_Coordinate.tickness),
                        m_Coordinate.tickness, m_ThemeInfo.axisLineColor);
                }
                else
                {
                    ChartHelper.DrawLine(vh, new Vector3(zeroX, coordinateY - m_Coordinate.tickness),
                        new Vector3(zeroX, coordinateY + coordinateHig + m_Coordinate.tickness),
                        m_Coordinate.tickness, m_ThemeInfo.axisLineColor);
                }

            }
            if (m_XAxis.show)
            {
                if (m_XAxis.type == Axis.AxisType.Value)
                {
                    ChartHelper.DrawLine(vh, new Vector3(coordinateX - m_Coordinate.tickness, coordinateY),
                        new Vector3(coordinateX + coordinateWid + m_Coordinate.tickness, coordinateY),
                        m_Coordinate.tickness, m_ThemeInfo.axisLineColor);
                }
                else
                {
                    ChartHelper.DrawLine(vh, new Vector3(coordinateX - m_Coordinate.tickness, zeroY),
                        new Vector3(coordinateX + coordinateWid + m_Coordinate.tickness, zeroY),
                        m_Coordinate.tickness, m_ThemeInfo.axisLineColor);
                }
            }
            #endregion
        }

        private void DrawDataZoom(VertexHelper vh)
        {
            if (!m_DataZoom.show) return;
            var p1 = new Vector2(coordinateX, m_DataZoom.bottom);
            var p2 = new Vector2(coordinateX, m_DataZoom.bottom + m_DataZoom.height);
            var p3 = new Vector2(coordinateX + coordinateWid, m_DataZoom.bottom + m_DataZoom.height);
            var p4 = new Vector2(coordinateX + coordinateWid, m_DataZoom.bottom);
            ChartHelper.DrawLine(vh, p1, p2, m_Coordinate.tickness, m_ThemeInfo.dataZoomLineColor);
            ChartHelper.DrawLine(vh, p2, p3, m_Coordinate.tickness, m_ThemeInfo.dataZoomLineColor);
            ChartHelper.DrawLine(vh, p3, p4, m_Coordinate.tickness, m_ThemeInfo.dataZoomLineColor);
            ChartHelper.DrawLine(vh, p4, p1, m_Coordinate.tickness, m_ThemeInfo.dataZoomLineColor);
            if (m_DataZoom.showDataShadow && m_Series.Count > 0)
            {
                Serie serie = m_Series.series[0];
                float scaleWid = coordinateWid / (serie.data.Count - 1);
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;
                for (int i = 0; i < serie.data.Count; i++)
                {
                    float value = serie.data[i];
                    float pX = zeroX + i * scaleWid;
                    float dataHig = value / (maxValue - minValue) * m_DataZoom.height;
                    np = new Vector3(pX, m_DataZoom.bottom + dataHig);
                    if (i > 0)
                    {
                        Color color = m_ThemeInfo.dataZoomLineColor;
                        ChartHelper.DrawLine(vh, lp, np, m_Coordinate.tickness, color);
                        Vector3 alp = new Vector3(lp.x, lp.y - m_Coordinate.tickness);
                        Vector3 anp = new Vector3(np.x, np.y - m_Coordinate.tickness);
                        Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                        Vector3 tnp = new Vector3(np.x, m_DataZoom.bottom + m_Coordinate.tickness);
                        Vector3 tlp = new Vector3(lp.x, m_DataZoom.bottom + m_Coordinate.tickness);
                        ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                    }
                    lp = np;
                }
            }
            switch (m_DataZoom.rangeMode)
            {
                case DataZoom.RangeMode.Percent:
                    var start = coordinateX + coordinateWid * m_DataZoom.start / 100;
                    var end = coordinateX + coordinateWid * m_DataZoom.end / 100;
                    p1 = new Vector2(start, m_DataZoom.bottom);
                    p2 = new Vector2(start, m_DataZoom.bottom + m_DataZoom.height);
                    p3 = new Vector2(end, m_DataZoom.bottom + m_DataZoom.height);
                    p4 = new Vector2(end, m_DataZoom.bottom);
                    ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.dataZoomSelectedColor);
                    ChartHelper.DrawLine(vh, p1, p2, m_Coordinate.tickness, m_ThemeInfo.dataZoomSelectedColor);
                    ChartHelper.DrawLine(vh, p3, p4, m_Coordinate.tickness, m_ThemeInfo.dataZoomSelectedColor);
                    break;
            }
        }

        protected void DrawSplitLine(VertexHelper vh, bool isYAxis, Axis.SplitLineType type,
            Vector3 startPos, Vector3 endPos, Color color)
        {
            switch (type)
            {
                case Axis.SplitLineType.Dashed:
                case Axis.SplitLineType.Dotted:
                    var startX = startPos.x;
                    var startY = startPos.y;
                    var dashLen = type == Axis.SplitLineType.Dashed ? 6 : 2.5f;
                    var count = isYAxis ? (endPos.x - startPos.x) / (dashLen * 2) :
                        (endPos.y - startPos.y) / (dashLen * 2);
                    for (int i = 0; i < count; i++)
                    {
                        if (isYAxis)
                        {
                            var toX = startX + dashLen;
                            ChartHelper.DrawLine(vh, new Vector3(startX, startY), new Vector3(toX, startY),
                                m_Coordinate.tickness, color);
                            startX += dashLen * 2;
                        }
                        else
                        {
                            var toY = startY + dashLen;
                            ChartHelper.DrawLine(vh, new Vector3(startX, startY), new Vector3(startX, toY),
                                m_Coordinate.tickness, color);
                            startY += dashLen * 2;
                        }

                    }
                    break;
                case Axis.SplitLineType.Solid:
                    ChartHelper.DrawLine(vh, startPos, endPos, m_Coordinate.tickness, color);
                    break;
            }
        }

        private void CheckDataZoom()
        {
            if (raycastTarget != m_DataZoom.show)
            {
                raycastTarget = m_DataZoom.show;
            }
            if (!m_DataZoom.show) return;
            if (m_DataZoom.showDetail)
            {
                Vector2 local;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                    Input.mousePosition, null, out local))
                {
                    m_DataZoom.SetLabelActive(false);
                    return;
                }
                if (m_DataZoom.IsInSelectedZoom(local, coordinateX, coordinateWid)
                    || m_DataZoom.IsInStartZoom(local, coordinateX, coordinateWid)
                    || m_DataZoom.IsInEndZoom(local, coordinateX, coordinateWid))
                {
                    m_DataZoom.SetLabelActive(true);
                    RefreshDataZoomLabel();
                }
                else
                {
                    m_DataZoom.SetLabelActive(false);
                }
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            var pos = transform.InverseTransformPoint(eventData.position);
            if (m_DataZoom.IsInStartZoom(pos, coordinateX, coordinateWid))
            {
                m_DataZoom.isDraging = true;
                m_DataZoomStartDrag = true;
            }
            else if (m_DataZoom.IsInEndZoom(pos, coordinateX, coordinateWid))
            {
                m_DataZoom.isDraging = true;
                m_DataZoomEndDrag = true;
            }
            else if (m_DataZoom.IsInSelectedZoom(pos, coordinateX, coordinateWid))
            {
                m_DataZoom.isDraging = true;
                m_DataZoomDrag = true;
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            //Debug.LogError("drag");
            float deltaX = eventData.delta.x;
            float deltaPercent = deltaX / coordinateWid * 100;
            if (m_DataZoomStartDrag)
            {
                m_DataZoom.start += deltaPercent;
                if (m_DataZoom.start < 0)
                {
                    m_DataZoom.start = 0;
                }
                else if (m_DataZoom.start > m_DataZoom.end)
                {
                    m_DataZoom.start = m_DataZoom.end;
                    m_DataZoomEndDrag = true;
                    m_DataZoomStartDrag = false;
                }
                RefreshDataZoomLabel();
                RefreshChart();
            }
            else if (m_DataZoomEndDrag)
            {
                m_DataZoom.end += deltaPercent;
                if (m_DataZoom.end > 100)
                {
                    m_DataZoom.end = 100;
                }
                else if (m_DataZoom.end < m_DataZoom.start)
                {
                    m_DataZoom.end = m_DataZoom.start;
                    m_DataZoomStartDrag = true;
                    m_DataZoomEndDrag = false;
                }
                RefreshDataZoomLabel();
                RefreshChart();
            }
            else if (m_DataZoomDrag)
            {
                if (deltaPercent > 0)
                {
                    if (m_DataZoom.end + deltaPercent > 100)
                    {
                        deltaPercent = 100 - m_DataZoom.end;
                    }
                }
                else
                {
                    if (m_DataZoom.start + deltaPercent < 0)
                    {
                        deltaPercent = -m_DataZoom.start;
                    }
                }
                m_DataZoom.start += deltaPercent;
                m_DataZoom.end += deltaPercent;
                RefreshDataZoomLabel();
                RefreshChart();
            }
        }

        private void RefreshDataZoomLabel()
        {
            var startIndex = (int)((xAxis.data.Count - 1) * m_DataZoom.start / 100);
            var endIndex = (int)((xAxis.data.Count - 1) * m_DataZoom.end / 100);
            if (m_DataZoomLastStartIndex != startIndex || m_DataZoomLastEndIndex != endIndex)
            {
                m_DataZoomLastStartIndex = startIndex;
                m_DataZoomLastEndIndex = endIndex;
                if (xAxis.data.Count > 0)
                {
                    m_DataZoom.SetStartLabelText(xAxis.data[startIndex]);
                    m_DataZoom.SetEndLabelText(xAxis.data[endIndex]);
                }
                InitAxisX();
            }

            var start = coordinateX + coordinateWid * m_DataZoom.start / 100;
            var end = coordinateX + coordinateWid * m_DataZoom.end / 100;
            m_DataZoom.startLabel.transform.localPosition =
                new Vector3(start - 10, m_DataZoom.bottom + m_DataZoom.height / 2);
            m_DataZoom.endLabel.transform.localPosition =
                new Vector3(end + 10, m_DataZoom.bottom + m_DataZoom.height / 2);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (m_DataZoomDrag || m_DataZoomStartDrag || m_DataZoomEndDrag)
            {
                RefreshChart();
            }
            m_DataZoomDrag = false;
            m_DataZoomStartDrag = false;
            m_DataZoomEndDrag = false;
            m_DataZoom.isDraging = false;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            var localPos = transform.InverseTransformPoint(eventData.position);
            if (m_DataZoom.IsInStartZoom(localPos, coordinateX, coordinateWid) ||
                m_DataZoom.IsInEndZoom(localPos, coordinateX, coordinateWid))
            {
                return;
            }
            if (m_DataZoom.IsInZoom(localPos, coordinateX, coordinateWid)
                && !m_DataZoom.IsInSelectedZoom(localPos, coordinateX, coordinateWid))
            {
                var pointerX = localPos.x;
                var selectWidth = coordinateWid * (m_DataZoom.end - m_DataZoom.start) / 100;
                var startX = pointerX - selectWidth / 2;
                var endX = pointerX + selectWidth / 2;
                if (startX < coordinateX)
                {
                    startX = coordinateX;
                    endX = coordinateX + selectWidth;
                }
                else if (endX > coordinateX + coordinateWid)
                {
                    endX = coordinateX + coordinateWid;
                    startX = coordinateX + coordinateWid - selectWidth;
                }
                m_DataZoom.start = (startX - coordinateX) / coordinateWid * 100;
                m_DataZoom.end = (endX - coordinateX) / coordinateWid * 100;
                RefreshDataZoomLabel();
                RefreshChart();
            }
        }

        public override void OnScroll(PointerEventData eventData)
        {
            if (!m_DataZoom.show || m_DataZoom.zoomLock) return;
            float deltaPercent = Mathf.Abs(eventData.scrollDelta.y *
                m_DataZoom.scrollSensitivity / coordinateWid * 100);
            if (eventData.scrollDelta.y > 0)
            {
                if (m_DataZoom.end <= m_DataZoom.start) return;
                m_DataZoom.end -= deltaPercent;
                m_DataZoom.start += deltaPercent;
                if (m_DataZoom.end <= m_DataZoom.start)
                {
                    m_DataZoom.end = m_DataZoom.start;
                }
            }
            else
            {
                m_DataZoom.end += deltaPercent;
                m_DataZoom.start -= deltaPercent;
                if (m_DataZoom.end > 100) m_DataZoom.end = 100;
                if (m_DataZoom.start < 0) m_DataZoom.start = 0;
            }
            RefreshDataZoomLabel();
            RefreshChart();
        }
    }
}

