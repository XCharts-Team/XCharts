using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System;

namespace XCharts
{
    public class CoordinateChart : BaseChart
    {
        private static readonly string s_DefaultSplitNameY = "split_y";
        private static readonly string s_DefaultSplitNameX = "split_x";

        [SerializeField] protected Coordinate m_Coordinate = Coordinate.defaultCoordinate;
        [SerializeField] protected XAxis m_XAxis = XAxis.defaultXAxis;
        [SerializeField] protected YAxis m_YAxis = YAxis.defaultYAxis;

        private float m_ZeroXOffset;
        private float m_ZeroYOffset;

        private XAxis m_CheckXAxis = XAxis.defaultXAxis;
        private YAxis m_CheckYAxis = YAxis.defaultYAxis;
        private Coordinate m_CheckCoordinate = Coordinate.defaultCoordinate;
        private List<Text> m_SplitYTextList = new List<Text>();
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
            InitSplitX();
            InitSplitY();
        }

        protected override void Update()
        {
            base.Update();
            CheckYAxis();
            CheckXAxis();
            CheckMinMaxValue();
            CheckCoordinate();
        }

        protected override void Reset()
        {
            base.Reset();
            m_Coordinate = Coordinate.defaultCoordinate;
            m_XAxis = XAxis.defaultXAxis;
            m_YAxis = YAxis.defaultYAxis;
            InitSplitX();
            InitSplitY();
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            DrawCoordinate(vh);
        }

        protected override void CheckTootipArea(Vector2 local)
        {
            if (local.x < zeroX || local.x > zeroX + coordinateWid ||
                local.y < zeroY || local.y > zeroY + coordinateHig)
            {
                m_Tooltip.dataIndex = 0;
                RefreshTooltip();
            }
            else
            {
                if (m_XAxis.type == Axis.AxisType.Value)
                {
                    float splitWid = m_YAxis.GetDataWidth(coordinateHig);
                    for (int i = 0; i < m_YAxis.GetDataNumber(); i++)
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
                    float splitWid = m_XAxis.GetDataWidth(coordinateWid);
                    for (int i = 0; i < m_XAxis.GetDataNumber(); i++)
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
            m_Tooltip.SetActive(true);
            if (m_Series.Count == 1)
            {
                float value = m_Series.GetData(0, index);
                string txt = tempAxis.GetData(index) + ": " + value;
                m_Tooltip.UpdateContentText(txt);
            }
            else
            {
                StringBuilder sb = new StringBuilder(tempAxis.GetData(index));
                for (int i = 0; i < m_Series.Count; i++)
                {
                    if (m_Series.series[i].show)
                    {
                        string strColor = ColorUtility.ToHtmlStringRGBA(m_ThemeInfo.GetColor(i));
                        string key = m_Series.series[i].name;
                        float value = m_Series.series[i].data[index];
                        sb.Append("\n");
                        sb.AppendFormat("<color=#{0}>● </color>", strColor);
                        sb.AppendFormat("{0}: {1}", key, value);
                    }

                }
                m_Tooltip.UpdateContentText(sb.ToString());
            }
            if(m_XAxis.type == Axis.AxisType.Value)
            {
                float hig = (maxValue - minValue) * (m_Tooltip.pointerPos.x - zeroX) / coordinateWid;
                m_Tooltip.UpdateLabelText(hig.ToString("f2"),tempAxis.GetData(index));
                float splitWidth = m_YAxis.GetSplitWidth(coordinateHig);
                float py = zeroY + (m_Tooltip.dataIndex - 1) * splitWidth
                    + (m_YAxis.boundaryGap ? splitWidth / 2 : 0);
                Vector2 xLabelPos = new Vector2(m_Tooltip.pointerPos.x,coordinateY- 4 * m_Coordinate.tickness);
                Vector2 yLabelPos = new Vector2(coordinateX - 6 * m_Coordinate.tickness,py);
                m_Tooltip.UpdateLabelPos(xLabelPos, yLabelPos);
            }
            else
            {
                float hig = (maxValue - minValue) * (m_Tooltip.pointerPos.y - zeroY) / coordinateHig;
                m_Tooltip.UpdateLabelText(tempAxis.GetData(index), hig.ToString("f2"));
                float splitWidth = m_XAxis.GetSplitWidth(coordinateWid);
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
        }

        TextGenerationSettings GetTextSetting()
        {
            var setting = new TextGenerationSettings();
            var fontdata = FontData.defaultFontData;

            //setting.generationExtents = rectTransform.rect.size;
            setting.generationExtents = new Vector2(200.0F, 50.0F);
            setting.fontSize = 14;
            setting.textAnchor = TextAnchor.MiddleCenter;
            setting.scaleFactor = 1f;
            setting.color = Color.red;
            setting.font = m_ThemeInfo.font;
            setting.pivot = new Vector2(0.5f, 0.5f);
            setting.richText = false;
            setting.lineSpacing = 0;
            setting.fontStyle = FontStyle.Normal;
            setting.resizeTextForBestFit = false;
            setting.horizontalOverflow = HorizontalWrapMode.Overflow;
            setting.verticalOverflow = VerticalWrapMode.Overflow;

            return setting;

        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
            InitSplitX();
            InitSplitY();
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

        private void InitSplitY()
        {
            m_SplitYTextList.Clear();
            float splitWidth = m_YAxis.GetScaleWidth(coordinateHig);

            var titleObject = ChartHelper.AddObject(s_DefaultSplitNameY, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            titleObject.transform.localPosition = Vector3.zero;
            ChartHelper.HideAllObject(titleObject, s_DefaultSplitNameY);

            for (int i = 0; i < m_YAxis.GetSplitNumber(); i++)
            {
                Text txt = ChartHelper.AddTextObject(s_DefaultSplitNameY + i, titleObject.transform,
                    m_ThemeInfo.font, m_ThemeInfo.textColor, TextAnchor.MiddleRight, Vector2.zero,
                    Vector2.zero, new Vector2(1, 0.5f), new Vector2(m_Coordinate.left, 20),
                    m_Coordinate.fontSize, m_XAxis.textRotation);
                txt.transform.localPosition = GetSplitYPosition(splitWidth, i);
                txt.text = m_YAxis.GetScaleName(i, minValue, maxValue);
                txt.gameObject.SetActive(m_YAxis.show);
                m_SplitYTextList.Add(txt);
            }
        }

        public void InitSplitX()
        {
            m_SplitXTextList.Clear();
            float splitWidth = m_XAxis.GetScaleWidth(coordinateWid);

            var titleObject = ChartHelper.AddObject(s_DefaultSplitNameX, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            titleObject.transform.localPosition = Vector3.zero;
            ChartHelper.HideAllObject(titleObject, s_DefaultSplitNameX);

            for (int i = 0; i < m_XAxis.GetSplitNumber(); i++)
            {
                Text txt = ChartHelper.AddTextObject(s_DefaultSplitNameX + i, titleObject.transform,
                    m_ThemeInfo.font, m_ThemeInfo.textColor, TextAnchor.MiddleCenter, Vector2.zero,
                    Vector2.zero, new Vector2(1, 0.5f), new Vector2(splitWidth, 20),
                    m_Coordinate.fontSize, m_XAxis.textRotation);

                txt.transform.localPosition = GetSplitXPosition(splitWidth, i);
                txt.text = m_XAxis.GetScaleName(i, minValue, maxValue);
                txt.gameObject.SetActive(m_XAxis.show);
                m_SplitXTextList.Add(txt);
            }
        }

        private Vector3 GetSplitYPosition(float scaleWid, int i)
        {
            if (m_YAxis.boundaryGap)
            {
                return new Vector3(coordinateX - m_YAxis.axisTick.length - 2f,
                    coordinateY + (i + 0.5f) * scaleWid, 0);
            }
            else
            {
                return new Vector3(coordinateX - m_YAxis.axisTick.length - 2f,
                    coordinateY + i * scaleWid, 0);
            }
        }

        private Vector3 GetSplitXPosition(float scaleWid, int i)
        {
            if (m_XAxis.boundaryGap)
            {
                return new Vector3(coordinateX + (i + 1) * scaleWid,
                    coordinateY - m_XAxis.axisTick.length - 12, 0);
            }
            else
            {
                return new Vector3(coordinateX + (i + 1 - 0.5f) * scaleWid,
                    coordinateY - m_XAxis.axisTick.length - 12, 0);
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
            if (!m_CheckXAxis.Equals(m_XAxis))
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
                m_Series.GetMinMaxValue(out tempMinValue, out tempMaxValue);
            }
            if (m_XAxis.type == Axis.AxisType.Value)
            {
                switch (m_XAxis.minMaxType)
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
                        break;
                    case Axis.AxisMinMaxType.MinMax:
                        tempMinValue = ChartHelper.GetMinDivisibleValue(tempMinValue);
                        tempMaxValue = ChartHelper.GetMaxDivisibleValue(tempMaxValue);
                        break;
                    case Axis.AxisMinMaxType.Custom:
                        if (m_XAxis.min != 0) tempMinValue = m_XAxis.min;
                        if (m_XAxis.max != 0) tempMaxValue = m_XAxis.max;
                        break;
                }
            }
            else if (m_YAxis.type == Axis.AxisType.Value)
            {
                switch (m_YAxis.minMaxType)
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
                        break;
                    case Axis.AxisMinMaxType.MinMax:
                        tempMinValue = ChartHelper.GetMinDivisibleValue(tempMinValue);
                        tempMaxValue = ChartHelper.GetMaxDivisibleValue(tempMaxValue);
                        break;
                    case Axis.AxisMinMaxType.Custom:
                        if (m_YAxis.min != 0) tempMinValue = m_YAxis.min;
                        if (m_YAxis.max != 0) tempMaxValue = m_YAxis.max;
                        break;
                }

            }
            if (tempMinValue != minValue || tempMaxValue != maxValue)
            {
                minValue = tempMinValue;
                maxValue = tempMaxValue;
                if (m_XAxis.type == Axis.AxisType.Value)
                {
                    m_ZeroXOffset = minValue > 0 ? 0 :
                        Mathf.Abs(minValue) * (coordinateWid / (Mathf.Abs(minValue) + Mathf.Abs(maxValue)));
                    OnXMaxValueChanged();
                }
                else if (m_YAxis.type == Axis.AxisType.Value)
                {
                    m_ZeroYOffset = minValue > 0 ? 0 :
                        Mathf.Abs(minValue) * (coordinateHig / (Mathf.Abs(minValue) + Mathf.Abs(maxValue)));
                    OnYMaxValueChanged();
                }
                RefreshChart();
            }
        }

        protected virtual void OnCoordinateChanged()
        {
            InitSplitX();
            InitSplitY();
        }

        protected virtual void OnYAxisChanged()
        {
            InitSplitY();
        }

        protected virtual void OnXAxisChanged()
        {
            InitSplitX();
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            InitSplitX();
            InitSplitY();
        }

        protected override void OnYMaxValueChanged()
        {
            for (int i = 0; i < m_SplitYTextList.Count; i++)
            {
                m_SplitYTextList[i].text = m_YAxis.GetScaleName(i, minValue, maxValue);
            }
        }

        protected virtual void OnXMaxValueChanged()
        {
            for (int i = 0; i < m_SplitXTextList.Count; i++)
            {
                m_SplitXTextList[i].text = m_XAxis.GetScaleName(i, minValue, maxValue);
            }
        }

        private void DrawCoordinate(VertexHelper vh)
        {
            #region draw tick and splitline
            if (m_YAxis.show)
            {
                for (int i = 0; i < m_YAxis.GetScaleNumber(); i++)
                {
                    float pX = 0;
                    float pY = coordinateY + i * m_YAxis.GetScaleWidth(coordinateHig);
                    if (m_YAxis.boundaryGap && m_YAxis.axisTick.alignWithLabel)
                    {
                        pY -= m_YAxis.GetScaleWidth(coordinateHig) / 2;
                    }

                    if (m_YAxis.axisTick.show)
                    {
                        pX += zeroX - m_YAxis.axisTick.length - 2;
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
                for (int i = 0; i < m_XAxis.GetScaleNumber(); i++)
                {
                    float pX = coordinateX + i * m_XAxis.GetScaleWidth(coordinateWid);
                    float pY = 0;
                    if (m_XAxis.boundaryGap && m_XAxis.axisTick.alignWithLabel)
                    {
                        pX -= m_XAxis.GetScaleWidth(coordinateWid) / 2;
                    }
                    if (m_XAxis.axisTick.show)
                    {
                        pY += zeroY - m_XAxis.axisTick.length - 2;
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
    }
}

